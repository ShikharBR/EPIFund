using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Inview.Epi.EpiFund.Domain;
using Inview.Epi.EpiFund.Domain.Entity;
using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Business
{
    public class FileManager : IFileManager
    {
        //private IAmazonS3 _cient;
        private string _documentsBucketName;
        private string _imagesBucketName;

        private IEPIContextFactory _factory;

        private string _fileRoot = ConfigurationManager.AppSettings["DataRoot"];

        private static List<FileInfo> files;

        private static List<DirectoryInfo> folders;

        public FileManager()
        {
            //var region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["bucketRegion"]);
            //_client = new AmazonS3Client(region);

            _documentsBucketName = ConfigurationManager.AppSettings["docsBucketName"];
            _imagesBucketName = ConfigurationManager.AppSettings["imagesBucketName"];
        }

        public FileManager(IEPIContextFactory factory)
        {
            this._factory = factory;
        }

        public byte[] ConvertImageToBytes(Image img)
        {
            byte[] array = new byte[0];
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                img.Save(memoryStream, ImageFormat.Png);
                memoryStream.Close();
                array = memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    ((IDisposable)memoryStream).Dispose();
                }
            }
            return array;
        }

        public void CreateThumbnail(string filename, Guid assetId)
        {
            try
            {
                string folder = this.getFolder(FileType.Image);
                string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString());
                string str1 = Path.Combine(str, filename);
                if (File.Exists(str1))
                {
                    Image image = Image.FromFile(str1);
                    Bitmap bitmap = new Bitmap(100, 100);
                    Graphics graphic = Graphics.FromImage(bitmap);
                    try
                    {
                        graphic.SmoothingMode = SmoothingMode.HighQuality;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.DrawImage(image, new Rectangle(0, 0, 100, 100));
                    }
                    finally
                    {
                        if (graphic != null)
                        {
                            ((IDisposable)graphic).Dispose();
                        }
                    }
                    string str2 = Path.Combine(str, string.Concat(Path.GetFileNameWithoutExtension(filename), "-thumb", Path.GetExtension(filename)));
                    bitmap.Save(str2);
                }
            }
            catch
            {
            }
        }

        public int CreateUserFile(HttpPostedFileBase file, int userId, UploadUserFileType? type)
        {
            IEPIRepository ePIRepository = this._factory.Create();
            string str = Path.Combine(this._fileRoot, "Documents", userId.ToString());
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }
            str = Path.Combine(str, string.Concat(Guid.NewGuid(), Path.GetExtension(file.FileName)));
            file.SaveAs(str);
            UserFile userFile = new UserFile()
            {
                DateUploaded = DateTime.Now,
                FileLocation = str,
                FileName = file.FileName,
                Type = type,
                UserId = userId
            };
            UserFile userFile1 = userFile;
            ePIRepository.UserFiles.Add(userFile1);
            ePIRepository.Save();
            return userFile1.UserFileId;
        }

        public bool DeleteFile(string fileName, Guid assetId, FileType filetype)
        {
            bool flag;
            try
            {
                string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], this.getFolder(filetype), assetId.ToString(), fileName);
                if (!this.fileOrDirectoryExists(str))
                {
                    flag = false;
                    return flag;
                }
                else if (filetype == FileType.Image)
                {
                    FileStream fileStream = new FileStream(str, FileMode.Open, FileAccess.Read);
                    try
                    {
                        Image.FromStream(fileStream);
                        fileStream.Dispose();
                    }
                    finally
                    {
                        if (fileStream != null)
                        {
                            ((IDisposable)fileStream).Dispose();
                        }
                    }
                    File.Delete(str);
                }
                else
                {
                    File.Delete(str);
                    flag = true;
                    return flag;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            flag = false;
            return flag;
        }

        public bool DeleteUserFile(int userFileId)
        {
            bool flag;
            UserFile userFile = this._factory.Create().UserFiles.FirstOrDefault<UserFile>((UserFile s) => s.UserFileId == userFileId);
            if (userFile != null)
            {
                if (File.Exists(userFile.FileLocation))
                {
                    File.Delete(userFile.FileLocation);
                    flag = true;
                    return flag;
                }
            }
            flag = false;
            return flag;
        }

        public virtual FilePathResult DownloadDocument(string fileName, string assetId, string contentType, string fileDownloadName)
        {
            FilePathResult filePathResult;
            string folder = this.getFolder(FileType.Document);
            try
            {
                fileDownloadName = string.Concat(fileDownloadName, Path.GetExtension(fileName));
            }
            catch
            {
            }
            if (!File.Exists(Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), fileName)))
            {
                filePathResult = null;
            }
            else
            {
                FilePathResult filePathResult1 = new FilePathResult(Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), fileName), contentType)
                {
                    FileDownloadName = fileDownloadName
                };
                filePathResult = filePathResult1;
            }
            return filePathResult;
        }

        internal bool fileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) ? true : File.Exists(name));
        }

        private static void FullDirList(DirectoryInfo dir, string searchPattern)
        {
            int i;
            try
            {
                FileInfo[] files = dir.GetFiles(searchPattern);
                for (i = 0; i < (int)files.Length; i++)
                {
                    FileInfo fileInfo = files[i];
                    FileManager.files.Add(fileInfo);
                }
            }
            catch
            {
                Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
                return;
            }
            DirectoryInfo[] directories = dir.GetDirectories();
            for (i = 0; i < (int)directories.Length; i++)
            {
                DirectoryInfo directoryInfo = directories[i];
                FileManager.folders.Add(directoryInfo);
                FileManager.FullDirList(directoryInfo, searchPattern);
            }
        }

        public string GetAssetImages()
        {
            FileManager.files = new List<FileInfo>();
            FileManager.folders = new List<DirectoryInfo>();
            string str = string.Concat(ConfigurationManager.AppSettings["DataRoot"], "\\images\\");
            FileManager.FullDirList(new DirectoryInfo(str), "*");
            IEPIRepository ePIRepository = this._factory.Create();
            FileManager.files.RemoveAll((FileInfo w) => w.Name.ToLower().IndexOf("thumb") > 1);
            List<FileInfo> list = (
                from x in FileManager.files
                join f in FileManager.folders on x.DirectoryName.ToLower().Trim() equals f.FullName.ToLower().Trim()
                join y in ePIRepository.AssetImages on new { fil = x.Name.ToString().ToLower().Trim(), dir = f.Name.ToString().ToLower().Trim() } equals new { fil = y.FileName.ToString().ToLower().Trim(), dir = y.AssetId.ToString().ToLower().Trim() }
                join z in ePIRepository.Assets on f.Name.ToString().ToLower().Trim() equals z.AssetId.ToString().ToLower().Trim()
                select x).DefaultIfEmpty<FileInfo>().ToList<FileInfo>();
            List<FileInfo> fileInfos = (
                from o in FileManager.files.Except<FileInfo>(list)
                orderby o.DirectoryName
                select o).ToList<FileInfo>();
            string str1 = "";
            foreach (FileInfo fileInfo in fileInfos)
            {
                int num = (
                    from o in ePIRepository.AssetImages
                    where o.AssetId.ToString().ToLower().Trim() == fileInfo.DirectoryName.ToString().ToLower().Trim()
                    select o).Count<AssetImage>();
                string name = (
                    from o in FileManager.folders
                    where o.FullName.ToLower().Trim() == fileInfo.DirectoryName.ToLower().Trim()
                    select o).FirstOrDefault<DirectoryInfo>().Name;
                name = (new Guid(name)).ToString();
                string str2 = str1;
                string[] strArrays = new string[] { str2, "Select '", null, null, null, null, null, null, null, null, null, null };
                Guid guid = Guid.NewGuid();
                strArrays[2] = guid.ToString().ToUpper().Trim();
                strArrays[3] = "' as AssetImageId,'";
                strArrays[4] = fileInfo.Name;
                strArrays[5] = "' as FileName , ";
                strArrays[6] = (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".jpeg" ? "'image/jpeg'" : "'image/png'");
                strArrays[7] = " as ContentType, ";
                strArrays[8] = num.ToString();
                strArrays[9] = " as [Order], 0 as IsFlyerImage, 0 as IsMainImage, '";
                strArrays[10] = name.ToUpper().Trim();
                strArrays[11] = "' as AssetId, null as OriginalFileName, null as Size union all ";
                str1 = string.Concat(strArrays);
            }
            if (str1.Length > 0)
            {
                str1 = str1.Substring(0, str1.Length - 11);
            }
            return str1;
        }

        public string GetFileSize(int byteSize)
        {
            int num = 1024;
            FileSize fileSize = FileSize.Bytes;
            while (byteSize > num)
            {
                byteSize /= num;
                fileSize++;
            }
            string str = string.Concat(byteSize, fileSize.ToString());
            return str;
        }

        internal string getFolder(FileType filetype)
        {
            string str;
            switch (filetype)
            {
                case FileType.Image:
                    {
                        str = "Images";
                        break;
                    }
                case FileType.Document:
                    {
                        str = "Documents";
                        break;
                    }
                case FileType.Video:
                    {
                        str = "Videos";
                        break;
                    }
                case FileType.TempImage:
                    {
                        str = "TempImages";
                        break;
                    }
                default:
                    {
                        str = "Documents";
                        break;
                    }
            }
            return str;
        }

        public virtual FilePathResult GetImageSource(string fileName, string assetId, string contentType)
        {
            string folder = this.getFolder(FileType.Image);
            FilePathResult filePathResult = new FilePathResult(Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), fileName), contentType);
            return filePathResult;
        }

        public virtual FilePathResult GetMainImageSource(Guid assetId)
        {
            FilePathResult filePathResult;
            AssetImage assetImage = this._factory.Create().AssetImages.FirstOrDefault<AssetImage>((AssetImage s) => (s.AssetId == assetId) && s.IsMainImage);
            if (assetImage == null)
            {
                filePathResult = null;
            }
            else
            {
                string folder = this.getFolder(FileType.Image);
                filePathResult = new FilePathResult(Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), assetImage.FileName), assetImage.ContentType);
            }
            return filePathResult;
        }

        private string GetMimeType(string extension)
        {
            char[] chrArray = Array.FindAll<char>(extension.ToCharArray(), (char c) => char.IsLetterOrDigit(c));
            extension = (new string(chrArray)).ToLower();
            Dictionary<string, string> strs = new Dictionary<string, string>()
            {
                { "ai", "application/postscript" },
                { "aif", "audio/x-aiff" },
                { "aifc", "audio/x-aiff" },
                { "aiff", "audio/x-aiff" },
                { "asc", "text/plain" },
                { "atom", "application/atom+xml" },
                { "au", "audio/basic" },
                { "avi", "video/x-msvideo" },
                { "bcpio", "application/x-bcpio" },
                { "bin", "application/octet-stream" },
                { "bmp", "image/bmp" },
                { "cdf", "application/x-netcdf" },
                { "cgm", "image/cgm" },
                { "class", "application/octet-stream" },
                { "cpio", "application/x-cpio" },
                { "cpt", "application/mac-compactpro" },
                { "csh", "application/x-csh" },
                { "css", "text/css" },
                { "dcr", "application/x-director" },
                { "dif", "video/x-dv" },
                { "dir", "application/x-director" },
                { "djv", "image/vnd.djvu" },
                { "djvu", "image/vnd.djvu" },
                { "dll", "application/octet-stream" },
                { "dmg", "application/octet-stream" },
                { "dms", "application/octet-stream" },
                { "doc", "application/msword" },
                { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
                { "docm", "application/vnd.ms-word.document.macroEnabled.12" },
                { "dotm", "application/vnd.ms-word.template.macroEnabled.12" },
                { "dtd", "application/xml-dtd" },
                { "dv", "video/x-dv" },
                { "dvi", "application/x-dvi" },
                { "dxr", "application/x-director" },
                { "eps", "application/postscript" },
                { "etx", "text/x-setext" },
                { "exe", "application/octet-stream" },
                { "ez", "application/andrew-inset" },
                { "gif", "image/gif" },
                { "gram", "application/srgs" },
                { "grxml", "application/srgs+xml" },
                { "gtar", "application/x-gtar" },
                { "hdf", "application/x-hdf" },
                { "hqx", "application/mac-binhex40" },
                { "htm", "text/html" },
                { "html", "text/html" },
                { "ice", "x-conference/x-cooltalk" },
                { "ico", "image/x-icon" },
                { "ics", "text/calendar" },
                { "ief", "image/ief" },
                { "ifb", "text/calendar" },
                { "iges", "model/iges" },
                { "igs", "model/iges" },
                { "jnlp", "application/x-java-jnlp-file" },
                { "jp2", "image/jp2" },
                { "jpe", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "jpg", "image/jpeg" },
                { "js", "application/x-javascript" },
                { "kar", "audio/midi" },
                { "latex", "application/x-latex" },
                { "lha", "application/octet-stream" },
                { "lzh", "application/octet-stream" },
                { "m3u", "audio/x-mpegurl" },
                { "m4a", "audio/mp4a-latm" },
                { "m4b", "audio/mp4a-latm" },
                { "m4p", "audio/mp4a-latm" },
                { "m4u", "video/vnd.mpegurl" },
                { "m4v", "video/x-m4v" },
                { "mac", "image/x-macpaint" },
                { "man", "application/x-troff-man" },
                { "mathml", "application/mathml+xml" },
                { "me", "application/x-troff-me" },
                { "mesh", "model/mesh" },
                { "mid", "audio/midi" },
                { "midi", "audio/midi" },
                { "mif", "application/vnd.mif" },
                { "mov", "video/quicktime" },
                { "movie", "video/x-sgi-movie" },
                { "mp2", "audio/mpeg" },
                { "mp3", "audio/mpeg" },
                { "mp4", "video/mp4" },
                { "mpe", "video/mpeg" },
                { "mpeg", "video/mpeg" },
                { "mpg", "video/mpeg" },
                { "mpga", "audio/mpeg" },
                { "ms", "application/x-troff-ms" },
                { "msh", "model/mesh" },
                { "mxu", "video/vnd.mpegurl" },
                { "nc", "application/x-netcdf" },
                { "oda", "application/oda" },
                { "ogg", "application/ogg" },
                { "pbm", "image/x-portable-bitmap" },
                { "pct", "image/pict" },
                { "pdb", "chemical/x-pdb" },
                { "pdf", "application/pdf" },
                { "pgm", "image/x-portable-graymap" },
                { "pgn", "application/x-chess-pgn" },
                { "pic", "image/pict" },
                { "pict", "image/pict" },
                { "png", "image/png" },
                { "pnm", "image/x-portable-anymap" },
                { "pnt", "image/x-macpaint" },
                { "pntg", "image/x-macpaint" },
                { "ppm", "image/x-portable-pixmap" },
                { "ppt", "application/vnd.ms-powerpoint" },
                { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
                { "potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
                { "ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
                { "ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12" },
                { "pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12" },
                { "potm", "application/vnd.ms-powerpoint.template.macroEnabled.12" },
                { "ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" },
                { "ps", "application/postscript" },
                { "qt", "video/quicktime" },
                { "qti", "image/x-quicktime" },
                { "qtif", "image/x-quicktime" },
                { "ra", "audio/x-pn-realaudio" },
                { "ram", "audio/x-pn-realaudio" },
                { "ras", "image/x-cmu-raster" },
                { "rdf", "application/rdf+xml" },
                { "rgb", "image/x-rgb" },
                { "rm", "application/vnd.rn-realmedia" },
                { "roff", "application/x-troff" },
                { "rtf", "text/rtf" },
                { "rtx", "text/richtext" },
                { "sgm", "text/sgml" },
                { "sgml", "text/sgml" },
                { "sh", "application/x-sh" },
                { "shar", "application/x-shar" },
                { "silo", "model/mesh" },
                { "sit", "application/x-stuffit" },
                { "skd", "application/x-koan" },
                { "skm", "application/x-koan" },
                { "skp", "application/x-koan" },
                { "skt", "application/x-koan" },
                { "smi", "application/smil" },
                { "smil", "application/smil" },
                { "snd", "audio/basic" },
                { "so", "application/octet-stream" },
                { "spl", "application/x-futuresplash" },
                { "src", "application/x-wais-source" },
                { "sv4cpio", "application/x-sv4cpio" },
                { "sv4crc", "application/x-sv4crc" },
                { "svg", "image/svg+xml" },
                { "swf", "application/x-shockwave-flash" },
                { "t", "application/x-troff" },
                { "tar", "application/x-tar" },
                { "tcl", "application/x-tcl" },
                { "tex", "application/x-tex" },
                { "texi", "application/x-texinfo" },
                { "texinfo", "application/x-texinfo" },
                { "tif", "image/tiff" },
                { "tiff", "image/tiff" },
                { "tr", "application/x-troff" },
                { "tsv", "text/tab-separated-values" },
                { "txt", "text/plain" },
                { "ustar", "application/x-ustar" },
                { "vcd", "application/x-cdlink" },
                { "vrml", "model/vrml" },
                { "vxml", "application/voicexml+xml" },
                { "wav", "audio/x-wav" },
                { "wbmp", "image/vnd.wap.wbmp" },
                { "wbmxl", "application/vnd.wap.wbxml" },
                { "wml", "text/vnd.wap.wml" },
                { "wmlc", "application/vnd.wap.wmlc" },
                { "wmls", "text/vnd.wap.wmlscript" },
                { "wmlsc", "application/vnd.wap.wmlscriptc" },
                { "wrl", "model/vrml" },
                { "xbm", "image/x-xbitmap" },
                { "xht", "application/xhtml+xml" },
                { "xhtml", "application/xhtml+xml" },
                { "xls", "application/vnd.ms-excel" },
                { "xml", "application/xml" },
                { "xpm", "image/x-xpixmap" },
                { "xsl", "application/xml" },
                { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { "xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
                { "xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12" },
                { "xltm", "application/vnd.ms-excel.template.macroEnabled.12" },
                { "xlam", "application/vnd.ms-excel.addin.macroEnabled.12" },
                { "xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12" },
                { "xslt", "application/xslt+xml" },
                { "xul", "application/vnd.mozilla.xul+xml" },
                { "xwd", "image/x-xwindowdump" },
                { "xyz", "chemical/x-xyz" },
                { "zip", "application/zip" }
            };
            Dictionary<string, string> strs1 = strs;
            return ((extension.Length <= 0 ? true : !strs1.ContainsKey(extension)) ? "" : strs1[extension]);
        }

        public virtual ThumbnailViewModel GetTempImageThumbnailByte(string filename, string assetId, string dateString, string userId, int thumbnailHeight, int thumbnailWidth)
        {
            ThumbnailViewModel thumbnailViewModel;
            string folder = this.getFolder(FileType.TempImage);
            string item = ConfigurationManager.AppSettings["DataRoot"];
            string[] strArrays = new string[] { dateString, "~", userId, "~", assetId.ToString() };
            string str = Path.Combine(item, folder, string.Concat(strArrays), filename);
            if (!File.Exists(str))
            {
                thumbnailViewModel = null;
            }
            else
            {
                ThumbnailViewModel bytes = new ThumbnailViewModel();
                Image image = Image.FromFile(str);
                bytes.Bytes = this.ConvertImageToBytes(image.GetThumbnailImage(thumbnailWidth, thumbnailHeight, null, IntPtr.Zero));
                bytes.ContentType = this.GetMimeType(Path.GetExtension(filename));
                thumbnailViewModel = bytes;
            }
            return thumbnailViewModel;
        }

        public virtual byte[] GetThumbnailByte(string filename, string assetId, int thumbnailHeight, int thumbnailWidth)
        {
            byte[] bytes;
            string folder = this.getFolder(FileType.Image);
            string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), filename);
            if (!File.Exists(str))
            {
                bytes = null;
            }
            else
            {
                Image image = Image.FromFile(str);
                bytes = this.ConvertImageToBytes(image.GetThumbnailImage(thumbnailWidth, thumbnailHeight, null, IntPtr.Zero));
            }
            return bytes;
        }

        public virtual byte[] GetViewAssetImage(string filename, string assetId, string contentType, int thumbnailHeight, int thumbnailWidth)
        {
            byte[] bytes;
            string folder = this.getFolder(FileType.Image);
            string str = Path.Combine(ConfigurationManager.AppSettings["DataRoot"], folder, assetId.ToString(), filename);
            if (!File.Exists(str))
            {
                bytes = null;
            }
            else
            {
                Image image = Image.FromFile(str);
                bytes = this.ConvertImageToBytes(FileManager.ScaleImage(image, thumbnailWidth, thumbnailHeight));
            }
            return bytes;
        }

        public string MoveTempAssetImage(string fileName, Guid assetId, string dateString, string userId)
        {
            string str;
            try
            {
                Guid guid = Guid.NewGuid();
                string str1 = string.Concat(guid.ToString(), Path.GetExtension(fileName));
                string str2 = Path.Combine(this._fileRoot, this.getFolder(FileType.Image), assetId.ToString());
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                string str3 = this._fileRoot;
                string folder = this.getFolder(FileType.TempImage);
                string[] strArrays = new string[] { dateString, "~", userId, "~", assetId.ToString() };
                File.Move(Path.Combine(str3, folder, string.Concat(strArrays), fileName), Path.Combine(str2, str1));
                str = str1;
            }
            catch (Exception exception)
            {
                str = null;
            }
            return str;
        }

        public string SaveFile(HttpPostedFileBase file, Guid assetId, FileType fileType)
        {
            string str;
            try
            {
                string folder = this.getFolder(fileType);
                string str1 = Path.Combine(this._fileRoot, folder, assetId.ToString());
                if (!Directory.Exists(str1))
                {
                    Directory.CreateDirectory(str1);
                }
                string str2 = string.Concat(Guid.NewGuid(), Path.GetExtension(file.FileName));
                str1 = Path.Combine(str1, str2);
                file.SaveAs(str1);
                str = str2;
            }
            catch (Exception exception)
            {
                str = null;
            }
            return str;
        }

        public bool SaveTempFile(HttpPostedFileBase file, Guid assetId, FileType fileType, string dateString, string userId)
        {
            bool flag;
            try
            {
                string folder = this.getFolder(fileType);
                string str = this._fileRoot;
                string[] strArrays = new string[] { dateString, "~", userId, "~", assetId.ToString() };
                string str1 = Path.Combine(str, folder, string.Concat(strArrays));
                if (!Directory.Exists(str1))
                {
                    Directory.CreateDirectory(str1);
                }
                str1 = Path.Combine(str1, file.FileName);
                file.SaveAs(str1);
            }
            catch (Exception exception)
            {
                flag = false;
                return flag;
            }
            flag = true;
            return flag;
        }

        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double num = (double)maxWidth / (double)image.Width;
            double num1 = (double)maxHeight / (double)image.Height;
            double num2 = Math.Min(num, num1);
            int width = (int)((double)image.Width * num2);
            int height = (int)((double)image.Height * num2);
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(bitmap);
            try
            {
                graphic.DrawImage(image, 0, 0, width, height);
            }
            finally
            {
                if (graphic != null)
                {
                    ((IDisposable)graphic).Dispose();
                }
            }
            return bitmap;
        }


        public byte[] GetScaledImageBytes(FileType fileType, Guid id, string fileName, int maxWidth, int maxHeight)
        {
            if (fileType != FileType.Image && fileType != FileType.TempImage)
            {
                throw new Exception("fileType must be Image or TempImage");
            }

            var key = GenerateS3Key(fileType, id.ToString(), fileName);

            var response = GetFile(_imagesBucketName, key);

            var format = GetImageFormat(response.Headers["Content-Type"]);
            Image image;

            using (Stream responseStream = response.ResponseStream)
            {
                image = Image.FromStream(responseStream);
            }

            var scaledImage = ScaleImage(image, maxWidth, maxHeight);
            var thumbnailName = String.Format("{0}-thumb{1}", Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));

            return ConvertImageToBytes(scaledImage, format);
        }

        private string GenerateS3Key(FileType type, string id, string name)
        {
            return (type == FileType.Image) ? String.Format("{0}/{1}", id, name) : String.Format("{0}/{1}", id, name);
        }

        private GetObjectResponse GetFile(string bucket, string key)
        {
            try
            {
                var req = new GetObjectRequest()
                {
                    BucketName = bucket,
                    Key = key
                };

                var response = new GetObjectResponse();//_client.GetObject(req);

                return response;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading an object", e.Message);
            }

            return null;
        }

        public ImageFormat GetImageFormat(string contentType)
        {
            switch (contentType)
            {
                case "image/bmp":
                    return ImageFormat.Bmp;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/x-icon":
                    return ImageFormat.Icon;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/png":
                    return ImageFormat.Png;
                case "image/tiff":
                    return ImageFormat.Tiff;
                default:
                    return null;
            }
        }
        public byte[] ConvertImageToBytes(Image image, ImageFormat format)
        {
            byte[] array = new byte[0];

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                array = ms.ToArray();
            }

            return array;
        }
    }
}