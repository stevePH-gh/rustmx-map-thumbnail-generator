using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RustedWarfareTMXViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            btnSave.Visible = false;
            tInfo.Visible = false;
            mapView.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public class LoadedTileset
        {
            public int FirstGid { get; set; }
            public string ImagePath { get; set; }
            public Bitmap Image { get; set; }
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public int Columns => (Image != null && TileWidth > 0) ? Image.Width / TileWidth : 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void PassFileName(String text)
        {
            String exportFileName = text;
            tOutput.Text = exportFileName.Substring(0, exportFileName.Length - 4);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            tInfo.Visible = false;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "TMX Files (*.tmx)|*.tmx|All Files (*.*)|*.*";
                ofd.Title = "Select a TMX Map";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    tOutput.ForeColor = Color.White;
                    String fileName = ofd.SafeFileName;
                    PassFileName(fileName);

                    if (mapView.Image != null)
                    {
                        mapView.Image.Dispose();
                        mapView.Image = null;
                    }

                    btnSave.Visible = false;

                    Bitmap renderedMap = RenderTmxMap(ofd.FileName);

                    if (renderedMap != null)
                    {
                        mapView.Image = renderedMap;

                        btnSave.Visible = true;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (mapView.Image == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                String mapName = tOutput.Text;

                sfd.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg";
                sfd.Title = "Export map thumbnail";
                sfd.FileName = mapName + "_map.png";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    tInfo.Visible = true;
                    using (Bitmap resizedMap = ResizeImage(mapView.Image, 500, 500))
                    {
                        ImageFormat format = sfd.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                            ? ImageFormat.Jpeg
                            : ImageFormat.Png;

                        resizedMap.Save(sfd.FileName, format);
                    }

                    MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            Bitmap destImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(destImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.DrawImage(image, 0, 0, width, height);
            }

            return destImage;
        }


        // ============================================================
        // ============================================== END SECTION
        // ============================================================

        /* CHATGPT */

        private Bitmap RenderTmxMap(string tmxFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(tmxFilePath);

            XmlNode mapNode = doc.SelectSingleNode("/map");
            if (mapNode == null) return null;

            int mapWidth = int.Parse(mapNode.Attributes["width"].Value);
            int mapHeight = int.Parse(mapNode.Attributes["height"].Value);
            int mapTileWidth = int.Parse(mapNode.Attributes["tilewidth"].Value);
            int mapTileHeight = int.Parse(mapNode.Attributes["tileheight"].Value);

            List<LoadedTileset> tilesets = new List<LoadedTileset>();

            XmlNodeList tilesetNodes = mapNode.SelectNodes("tileset");
            foreach (XmlNode tsNode in tilesetNodes)
            {
                int firstGid = int.Parse(tsNode.Attributes["firstgid"].Value);
                string tsxSource = tsNode.Attributes["source"]?.Value;

                Bitmap tileBitmap = null;
                int tsTileWidth = mapTileWidth;
                int tsTileHeight = mapTileHeight;

                if (tsNode.Attributes["tilewidth"] != null)
                    tsTileWidth = int.Parse(tsNode.Attributes["tilewidth"].Value);
                if (tsNode.Attributes["tileheight"] != null)
                    tsTileHeight = int.Parse(tsNode.Attributes["tileheight"].Value);

                // 1. Check for Base64 embedded_png anywhere inside the tileset tag
                XmlNode embeddedProperty = tsNode.SelectSingleNode(".//property[@name='embedded_png']");
                if (embeddedProperty != null && embeddedProperty.Attributes["value"] != null)
                {
                    string base64String = embeddedProperty.Attributes["value"].Value;
                    tileBitmap = LoadBitmapFromBase64(base64String);
                }

                // 2. Fallback: Check external .tsx file
                if (tileBitmap == null && !string.IsNullOrEmpty(tsxSource))
                {
                    string tsxFileName = Path.GetFileName(tsxSource);
                    using (Stream tsxStream = GetEmbeddedStream(tsxFileName))
                    {
                        if (tsxStream != null)
                        {
                            XmlDocument tsxDoc = new XmlDocument();
                            tsxDoc.Load(tsxStream);

                            XmlNode tsxRoot = tsxDoc.SelectSingleNode("/tileset");
                            if (tsxRoot != null)
                            {
                                if (tsxRoot.Attributes["tilewidth"] != null)
                                    tsTileWidth = int.Parse(tsxRoot.Attributes["tilewidth"].Value);
                                if (tsxRoot.Attributes["tileheight"] != null)
                                    tsTileHeight = int.Parse(tsxRoot.Attributes["tileheight"].Value);
                            }

                            XmlNode tsxEmbedded = tsxDoc.SelectSingleNode("//property[@name='embedded_png']");
                            if (tsxEmbedded != null && tsxEmbedded.Attributes["value"] != null)
                            {
                                tileBitmap = LoadBitmapFromBase64(tsxEmbedded.Attributes["value"].Value);
                            }
                            else
                            {
                                XmlNode tsxImageNode = tsxDoc.SelectSingleNode("//image");
                                if (tsxImageNode != null)
                                {
                                    string imageName = tsxImageNode.Attributes["source"].Value;
                                    tileBitmap = GetEmbeddedBitmap(imageName);
                                }
                            }
                        }
                    }
                }

                // 3. Fallback: Standard image tag inside the map file
                if (tileBitmap == null)
                {
                    XmlNode imgNode = tsNode.SelectSingleNode("image");
                    if (imgNode != null)
                    {
                        string imageName = imgNode.Attributes["source"].Value;
                        tileBitmap = GetEmbeddedBitmap(imageName);
                    }
                }

                if (tileBitmap != null)
                {
                    tilesets.Add(new LoadedTileset
                    {
                        FirstGid = firstGid,
                        Image = tileBitmap,
                        TileWidth = tsTileWidth,
                        TileHeight = tsTileHeight
                    });
                }
            }

            if (tilesets.Count == 0)
            {
                MessageBox.Show("Could not load any tilesets!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            tilesets = tilesets.OrderByDescending(t => t.FirstGid).ToList();





            try
            {
                // ============================================================================================================
                // ====================================== I ENCASED THIS IN try...catch TO AVOID FATAL ERROR IN BIGGER MAPS
                // ============================================================================================================


                Bitmap mapBitmap = new Bitmap(mapWidth * mapTileWidth, mapHeight * mapTileHeight);

                using (Graphics g = Graphics.FromImage(mapBitmap))
                {
                    g.Clear(Color.Black);

                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.PixelOffsetMode = PixelOffsetMode.Half;

                    XmlNodeList layerNodes = mapNode.SelectNodes("layer");
                    foreach (XmlNode layer in layerNodes)
                    {
                        XmlNode dataNode = layer.SelectSingleNode("data");
                        if (dataNode == null) continue;

                        string encoding = dataNode.Attributes["encoding"]?.Value;
                        string compression = dataNode.Attributes["compression"]?.Value;

                        uint[] gids = ParseLayerGids(dataNode.InnerText.Trim(), encoding, compression, mapWidth * mapHeight);
                        if (gids == null) continue;

                        for (int i = 0; i < gids.Length; i++)
                        {
                            uint rawGid = gids[i];
                            if (rawGid == 0) continue;

                            int gid = (int)(rawGid & 0x1FFFFFFF);

                            LoadedTileset ts = tilesets.FirstOrDefault(t => gid >= t.FirstGid);
                            if (ts == null || ts.Image == null || ts.Columns == 0) continue;

                            int tileId = gid - ts.FirstGid;
                            int srcX = (tileId % ts.Columns) * ts.TileWidth;
                            int srcY = (tileId / ts.Columns) * ts.TileHeight;

                            int destX = (i % mapWidth) * mapTileWidth;
                            int destY = (i / mapWidth) * mapTileHeight;

                            Rectangle srcRect = new Rectangle(srcX, srcY, ts.TileWidth, ts.TileHeight);
                            Rectangle destRect = new Rectangle(destX, destY, mapTileWidth, mapTileHeight);

                            g.DrawImage(ts.Image, destRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                }

                foreach (var ts in tilesets) ts.Image?.Dispose();

                return mapBitmap;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error!\nMap is simply too big", "Error");
                String storeFileName = tOutput.Text;
                tOutput.ForeColor = Color.Red;
                tOutput.Text = storeFileName + " - error";
            }

            return null; // I JUST ADDED THIS
        }

        /// <summary>
        /// Converts a Base64-encoded string from TMX property "embedded_png" into a Bitmap.
        /// </summary>
        private Bitmap LoadBitmapFromBase64(string base64Data)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Data.Trim());
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Bitmap temp = new Bitmap(ms))
                    {
                        return new Bitmap(temp); // Copy to detach from stream memory
                    }
                }
            }
            catch
            {
                return null;
            }
        }





        #region Embedded Resource Loaders

        /// <summary>
        /// Finds and opens an embedded resource stream by filename or partial path.
        /// </summary>
        private Stream GetEmbeddedStream(string pathOrFileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string cleanFileName = Path.GetFileName(pathOrFileName);

            // Search by exact end match of the filename
            string resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith(cleanFileName, StringComparison.OrdinalIgnoreCase));

            return resourceName != null ? assembly.GetManifestResourceStream(resourceName) : null;
        }

        /// <summary>
        /// Loads an embedded PNG or image asset as a standalone Bitmap.
        /// </summary>
        private Bitmap GetEmbeddedBitmap(string pathOrFileName)
        {
            using (Stream stream = GetEmbeddedStream(pathOrFileName))
            {
                if (stream == null) return null;

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    using (Bitmap temp = new Bitmap(ms))
                    {
                        return new Bitmap(temp); // Clone to detach from stream completely
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// Parses TMX layer data supporting CSV, Base64 uncompressed, and Base64 + Zlib compression formats.
        /// </summary>
        /// <summary>
        /// Parses TMX layer data supporting CSV, Base64 uncompressed, and Base64 + Zlib/GZip compression.
        /// </summary>
        private uint[] ParseLayerGids(string dataText, string encoding, string compression, int totalTiles)
        {
            if (encoding == "csv" || string.IsNullOrEmpty(encoding))
            {
                string[] tileGids = dataText.Split(new[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                uint[] result = new uint[tileGids.Length];
                for (int i = 0; i < tileGids.Length; i++)
                {
                    uint.TryParse(tileGids[i].Trim(), out result[i]);
                }
                return result;
            }

            if (encoding == "base64")
            {
                // Strip out all whitespace, newlines, and carriage returns
                string cleanBase64 = dataText.Replace("\r", "")
                                               .Replace("\n", "")
                                               .Replace(" ", "")
                                               .Trim();

                byte[] rawData = Convert.FromBase64String(cleanBase64);

                byte[] decompressedData;
                if (compression == "zlib")
                {
                    // Zlib wrapper format: 2-byte header + compressed DEFLATE payload + 4-byte Adler32 checksum
                    int offset = 0;
                    int length = rawData.Length;

                    // Skip 2-byte Zlib header if present (e.g., 0x78 0x9C, 0x78 0xDA, etc.)
                    if (rawData.Length > 6 && rawData[0] == 0x78)
                    {
                        offset = 2;
                        length -= 6; // Trim 2-byte header and 4-byte trailing Adler-32 checksum
                    }

                    using (MemoryStream compressedStream = new MemoryStream(rawData, offset, length))
                    using (DeflateStream deflate = new DeflateStream(compressedStream, CompressionMode.Decompress))
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        deflate.CopyTo(resultStream);
                        decompressedData = resultStream.ToArray();
                    }
                }
                else if (compression == "gzip")
                {
                    using (MemoryStream compressedStream = new MemoryStream(rawData))
                    using (GZipStream gzip = new GZipStream(compressedStream, CompressionMode.Decompress))
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        gzip.CopyTo(resultStream);
                        decompressedData = resultStream.ToArray();
                    }
                }
                else
                {
                    decompressedData = rawData;
                }

                uint[] gids = new uint[decompressedData.Length / 4];
                for (int i = 0; i < gids.Length; i++)
                {
                    gids[i] = BitConverter.ToUInt32(decompressedData, i * 4);
                }
                return gids;
            }

            return null;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
