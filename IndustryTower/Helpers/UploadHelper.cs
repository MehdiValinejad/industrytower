using IndustryTower.App_Start;
using IndustryTower.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace IndustryTower.Helpers
{
    public static class UploadHelper
    {
        #region static variables

        
        private static int W_FixedSize = 200;
        private static int H_FixedSize = 200;

        #endregion

        private enum ImageModificationType
        {
            Crop,
            Resize
        };

        public enum FileTypes
        {
            image,
            doc,
            xls,
            ppt,
            db
        }


        //public static bool FileIsWebFriendlyImage(Stream stream)
        //{
        //    try
        //    {
        //        //Read an image from the stream...
        //        var i = Image.FromStream(stream);
        //        //Move the pointer back to the beginning of the stream
        //        stream.Seek(0, SeekOrigin.Begin);
        //        if (ImageFormat.Jpeg.Equals(i.RawFormat))
        //            return true;
        //        return ImageFormat.Png.Equals(i.RawFormat)
        //        || ImageFormat.Gif.Equals(i.RawFormat);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}


        public static bool CheckExtenstionFast(string fileName,FileTypes type)
        {
            var ext = fileName.Substring(fileName.ToLower().LastIndexOf(".") + 1);
            switch (ext)
            {
                case "jpg":
                case "png":
                case "gif":
                case "jpeg":
                    return type == FileTypes.image;
                case "xls":
                case "xlsx":
                    return type == FileTypes.xls;
                case "pdf":
                case "doc":
                case "docx":
                    return type == FileTypes.doc;
                case "accdb":
                case "mdb":
                case "mda":
                    return type == FileTypes.db;
                case "pps":
                case "ppsx":
                case "ppt":
                    return type == FileTypes.ppt;
                default:
                    return false;
            }

        }


        // folder for the upload
        public static readonly string ItemUploadFolderPath = "~/Uploads/Temporary/";

        public static string renameUploadFile(HttpPostedFileBase file, int height, bool square = false, Int32 counter = 0, int? fileSizeLimit = null)
        {
            var fileName = Path.GetFileName(file.FileName);

            string guid = Guid.NewGuid().ToString();
            string append = DateTime.Now.Date.ToString("yyMMdd") + guid;
            string extension = Path.GetExtension(file.FileName);
            string finalFileName = append + ((counter).ToString()) + extension.ToLower();
            if (File.Exists(HttpContext.Current.Request.MapPath(ItemUploadFolderPath + finalFileName)))
            {
                //file exists 
                return renameUploadFile(file, height, square, ++counter);
            }
            //file doesn't exist, upload item but validate first
            return uploadFile(file, finalFileName, height, square, fileSizeLimit);
        }

        private static string uploadFile(HttpPostedFileBase file, string fileName, int height, bool square = false, int? fileSizeLimit = null)
        {
            fileSizeLimit = fileSizeLimit == null ? ITTConfig.FileSizeLimit : fileSizeLimit;
            var path = Path.Combine(HttpContext.Current.Request.MapPath(ItemUploadFolderPath), fileName);
            var fileSize = file.ContentLength;
            if (fileSize > fileSizeLimit)
            {
                throw new JsonCustomException(Resource.ControllerError.ajaxFileSize);
            }
            string extension = Path.GetExtension(file.FileName);
            string fileMimeType = file.ContentType;
            //make sure the file is valid
            if (!validateExtension(extension, fileMimeType))
            {
                throw new JsonCustomException(Resource.ControllerError.ajaxFileType);
            }
            
            try
            {

                if (IsImage(fileName))
                {
                    Image img = Image.FromStream(file.InputStream, true, true);
                    if (img.Height > height)
                    {
                        ResizeImageByFile(img, path, extension, height, square);
                    }
                    else
                    {
                        file.SaveAs(path);
                    }
                }
                else
                {
                    file.SaveAs(path);
                }
                

                //Image imgOriginal = Image.FromFile(path);

                ////pass in whatever value you want for the width (180)
                //Image imgActual = ScaleBySize(imgOriginal, 600);
                //imgOriginal.Dispose();
                //imgActual.Save(path);
                //imgActual.Dispose();

                return fileName;
            }
            catch
            {
                throw new JsonCustomException(Resource.ControllerError.ajaxFileUploadError);
            }
        }

        private static bool validateExtension(string extension, string mimeType)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return checkMime(extension, mimeType);
                case ".png":
                    return checkMime(extension, mimeType);
                case ".gif":
                    return checkMime(extension, mimeType);
                case ".jpeg":
                    return checkMime(extension, mimeType);
                case ".xls":
                    return checkMime(extension, mimeType);
                case ".xlsx":
                    return checkMime(extension, mimeType);
                case ".pdf":
                    return checkMime(extension, mimeType);
                case ".doc":
                    return checkMime(extension, mimeType);
                case ".docx":
                    return checkMime(extension, mimeType);
                case ".accdb":
                    return checkMime(extension, mimeType);
                case ".mdb":
                    return checkMime(extension, mimeType);
                case ".mda":
                    return checkMime(extension, mimeType);
                case ".pps":
                    return checkMime(extension, mimeType);
                case ".ppsx":
                    return checkMime(extension, mimeType);
                case ".ppt":
                    return checkMime(extension, mimeType);
                default:
                    return false;
            }
            
        }
        private static bool checkMime(string extension, string mimeType)
        {
            if (GetMimeType(extension).Any(s => s == mimeType))
                return true;
            else
                return false;
        }

        //public static bool DeleteFile(string IMGName, string folder)
        //{
        //    string IMGPath = HttpContext.Current.Request.MapPath(ItemUploadFolderPath +  folder + "/" + IMGName);
        //    if (File.Exists(IMGPath))
        //    {
        //        File.Delete(IMGPath);
        //        return true;
        //    }
        //    return false;
        //}




        public static string CropImage(int x, int y, int w, int h, string IMGName)
        {
            string IMGPath = HttpContext.Current.Request.MapPath(ItemUploadFolderPath + IMGName);
                
            try
            {
                if (w == 0 && h == 0) // Make sure the user selected a crop area
                    throw new Exception("A crop selection was not made.");

                string imageId = ModifyImage(x, y, w, h, ImageModificationType.Crop, IMGPath);
                return "Successfully Croped";
            }
            catch
            {
                //string errorMsg = string.Format("Error cropping image: {0}", ex.Message);

                throw new JsonCustomException(Resource.ControllerError.ajaxImageCropError);
            }
        }

        public static string ResizeImage(string IMGName, string folder)
        {
            string IMGPath = HttpContext.Current.Request.MapPath(ItemUploadFolderPath + "Images/" + folder + "/" + IMGName);
            try
            {
                string imageId = ModifyImage(0, 0, W_FixedSize, H_FixedSize, ImageModificationType.Resize, IMGPath);
                return "Successfully Resized";
            }
            catch
            {
                //string errorMsg = string.Format("Error resizing image: {0}", ex.Message);

                throw new JsonCustomException(Resource.ControllerError.ajaxImageResizeError);
            }
        }

 
        private static string ModifyImage(int x, int y, int w, int h, ImageModificationType modType, string IMGToModifypath)
        {
            
            
            Image img = Image.FromFile(IMGToModifypath);
            
            var WorkingImageExtension = Path.GetExtension(IMGToModifypath);

            using (System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(w, h))
            {
                _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    if (modType == ImageModificationType.Crop)
                    {
                        _graphic.DrawImage(img, 0, 0, w, h);
                        _graphic.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
                    }
                    else if (modType == ImageModificationType.Resize)
                    {
                        _graphic.DrawImage(img, 0, 0, img.Width, img.Height);
                        _graphic.DrawImage(img, new Rectangle(0, 0, W_FixedSize, H_FixedSize), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    }

                    string extension = WorkingImageExtension;

                    // If the image is a gif file, change it into png
                    if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = ".png";
                    }

                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                        img.Dispose();
                        ImageSave(IMGToModifypath, _bitmap, extension, encoderParameters);
                        
                    }
                }
            }

            return "Successful";
        }

        public static string ResizeImageByFile(Image img, string filePath, string extension, int height, bool square = false)
        {
            //string IMGPath = HttpContext.Current.Request.MapPath(ItemUploadFolderPath + "Images/" + folder + "/" + IMGName);

            int newWidth = 0;
            if (square)
            {
                float ratio = (float)img.Height / (float)img.Width;
                if (ratio > 1)
                {
                    var oldheigh = height;
                    newWidth = oldheigh;
                    height = (int)Math.Ceiling(ratio * height);
                }
                else
                {
                    newWidth = img.Width * height / img.Height;
                }
            }
            else
            {
                newWidth = img.Width * height / img.Height;
            }
            
            try
            {
                string imageId = ModifyImageByFile(0, 0, newWidth, height, ImageModificationType.Resize, img, filePath, extension);
                return "Successfully Resized";
            }
            catch
            {
                //string errorMsg = string.Format("Error resizing image: {0}", ex.Message);
                throw new JsonCustomException(Resource.ControllerError.ajaxImageResizeError);
               
            }
        }

        private static string ModifyImageByFile(int x, int y, int w, int h, ImageModificationType modType, Image img, string filePath, string ext)
        {

            
            //Image img = Image.FromFile(IMGToModifypath);

            var WorkingImageExtension = img.RawFormat.ToString();//Path.GetExtension(IMGToModifypath);

            using (System.Drawing.Bitmap _bitmap = new System.Drawing.Bitmap(w, h))
            {
                _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                using (Graphics _graphic = Graphics.FromImage(_bitmap))
                {
                    _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    if (modType == ImageModificationType.Crop)
                    {
                        _graphic.DrawImage(img, 0, 0, w, h);
                        _graphic.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
                    }
                    else if (modType == ImageModificationType.Resize)
                    {
                        _graphic.DrawImage(img, 0, 0, img.Width, img.Height);
                        _graphic.DrawImage(img, new Rectangle(0, 0, w, h), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    }

                    string extension = ext;

                    // If the image is a gif file, change it into png
                    if (extension.EndsWith("gif", StringComparison.OrdinalIgnoreCase))
                    {
                        extension = ".png";
                    }

                    using (EncoderParameters encoderParameters = new EncoderParameters(1))
                    {
                        
                        encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 40L);
                        img.Dispose();
                        ImageSave(filePath, _bitmap, extension, encoderParameters);

                    }
                }
            }

            return "Successful";
        }


        public static ImageCodecInfo GetImageCodec(string extension)
        {
            extension = extension.ToUpperInvariant();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.Contains(extension))
                {
                    return codec;
                }
            }
            return codecs[1];
        }

        public static string ImageSave(string path, System.Drawing.Image image)
        {
            return ImageSave(path, image, null, null);
        }

        public static string ImageSave(string path, System.Drawing.Image image, string extension, EncoderParameters encoderParameters)
        {
            
            if (!string.IsNullOrEmpty(extension) && encoderParameters != null)
                image.Save(path, GetImageCodec(extension), encoderParameters);
            else
                image.Save(path, image.RawFormat);
            return "Successfull";
        }


        public static bool moveFile(string fileNameToMove, string folder)
        {
            var fileNameToMovePath = HttpContext.Current.Request.MapPath(ItemUploadFolderPath + fileNameToMove);
            if (File.Exists(fileNameToMovePath))
            {

                //string[] img = { ".jpg", ".png", ".gif", ".jpeg" };
                //string[] video = { ".flv", ".wmv", ".3gp", ".mp4" };
                //string[] doc = { ".doc", ".docx" };
                //string[] pdf = { ".pdf" };
                //string[] xls = { ".xls", ".xlsx" };
                //string[] pps = { ".pps", ".ppsx", "ppt" };
                //string[] DB = { ".accdb", ".accdb", ".mdb", ".mda" };
                var ext = fileNameToMove.Substring(fileNameToMove.ToLower().LastIndexOf(".") + 1);
                var folderpath = HttpContext.Current.Request.MapPath(String.Concat("~/Uploads/", ext, "/", folder, "/"));
                if (Directory.Exists(folderpath))
                {
                    File.Move(fileNameToMovePath, String.Concat(folderpath,"\\",fileNameToMove));
                    return true;
                }
                else
                {
                    Directory.CreateDirectory(folderpath);
                    File.Move(fileNameToMovePath, String.Concat(folderpath, "\\", fileNameToMove));
                    return true;
                }
                //if (img.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Images/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (video.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Videos/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (doc.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Documents/DOC/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (pdf.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Documents/PDF/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (xls.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Documents/XLS/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (pps.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Documents/PPS/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else if (DB.Contains(ext))
                //{
                //    File.Move(fileNameToMovePath, HttpContext.Current.Request.MapPath("~/Uploads/Documents/DB/" + folder + "/" + fileNameToMove));
                //    return true;
                //}
                //else return false;

            }
            else return false;
        }

        public static bool deleteFile(string fileNameToDelete, string folder)
        {
            var ext = fileNameToDelete.Substring(fileNameToDelete.ToLower().LastIndexOf(".") + 1);
            var fileToDeletPath = HttpContext.Current.Request.MapPath(String.Concat("~/Uploads/",ext,"/",folder,"/",fileNameToDelete));
            if (File.Exists(fileToDeletPath))
            {
                File.Delete(fileToDeletPath);
                return true;
            }
            
            //string[] img = { ".jpg", ".png", ".gif", ".jpeg" };
            //string[] video = { ".flv", ".wmv", ".3gp", ".mp4" };
            //string[] doc = { ".doc", ".docx"};
            //string[] pdf = { ".pdf" };
            //string[] xls = { ".xls", ".xlsx", };
            //string[] pps = { ".pps", ".ppsx", "ppt" };
            //string[] DB = {  ".accdb", ".accdb", ".mdb", ".mda"  };

            //if (img.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Images/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;

            //}
            //else if (video.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Videos/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (doc.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Documents/DOC/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (pdf.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Documents/PDF/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (xls.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Documents/XLS/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (pps.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Documents/PPS/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            //else if (DB.Contains(ext))
            //{
            //    var fileToDeletPath = HttpContext.Current.Request.MapPath("~/Uploads/Documents/DB/" + folder + "/" + fileNameToDelete);
            //    if (File.Exists(fileToDeletPath))
            //    {
            //        File.Delete(fileToDeletPath);
            //        return true;
            //    }
            //    else return false;
            //}
            else return false;
        }


        //public static Image ScaleBySize(Image imgPhoto, int size)
        //{
        //    int logoSize = size;

        //    float sourceWidth = imgPhoto.Width;
        //    float sourceHeight = imgPhoto.Height;
        //    float destHeight = 0;
        //    float destWidth = 0;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    int destX = 0;
        //    int destY = 0;

        //    // Resize Image to have the height = logoSize/2 or width = logoSize.
        //    // Height is greater than width, set Height = logoSize and resize width accordingly
        //    if (sourceWidth > (2 * sourceHeight))
        //    {
        //        destWidth = logoSize;
        //        destHeight = (float)(sourceHeight * logoSize / sourceWidth);
        //    }
        //    else
        //    {
        //        int h = logoSize / 2;
        //        destHeight = h;
        //        destWidth = (float)(sourceWidth * h / sourceHeight);
        //    }
        //    // Width is greater than height, set Width = logoSize and resize height accordingly

        //    Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
        //                                PixelFormat.Format32bppPArgb);
        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        //    grPhoto.DrawImage(imgPhoto,
        //        new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
        //        new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
        //        GraphicsUnit.Pixel);

        //    grPhoto.Dispose();

        //    return bmPhoto;
        //}

        private static IDictionary<string, List<string>> _mappings = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types
        // combination of values from Windows 7 Registry and 
        // from C:\Windows\System32\inetsrv\config\applicationHost.config
        // some added, including .7z and .dat
        {".jpg", new List<string>{"image/jpeg","image/pjpeg"}},
        {".png", new List<string>{"image/png","image/x-png","image/x-citrix-png"}},
        {".gif", new List<string>{"image/gif"}},
        {".jpeg", new List<string>{"image/jpeg","image/pjpeg","x-citrix-jpeg"}},
        {".xls", new List<string>{"application/excel","application/vnd.ms-excel","application/x-excel","application/x-msexcel"}},
        {".xlsx", new List<string>{"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}},
        {".pdf", new List<string>{"application/pdf"}},
        {".doc", new List<string>{"application/msword"}},
        {".docx", new List<string>{"application/vnd.openxmlformats-officedocument.wordprocessingml.document"}},
        {".accdb", new List<string>{"application/msaccess"}},
        {".mdb", new List<string>{"application/x-msaccess"}},
        {".mda", new List<string>{"application/msaccess"}},
        {".pps", new List<string>{"application/mspowerpoint","application/vnd.ms-powerpoint"}},
        {".ppsx", new List<string>{"application/vnd.openxmlformats-officedocument.presentationml.slideshow"}},
        {".ppt", new List<string>{"application/mspowerpoint","application/powerpoint","application/vnd.ms-powerpoint","application/x-mspowerpoint"}}
        
        #endregion

        };

        public static List<string> GetMimeType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
             
            List<string> mime;
            return _mappings.TryGetValue(extension, out mime) ? mime : new List<string> { "application/octet-stream" };
        }


        public static UploadedFilesString UpdateUploadedFiles(string newFiles, string initialFiles, string folder)
        {
            UploadedFilesString Result = new UploadedFilesString();
            HashSet<string> filesHS = !String.IsNullOrWhiteSpace(newFiles) 
                                      ? new HashSet<string>(newFiles.Split(new char[] { ',' }))
                                      : new HashSet<string>();
            HashSet<string> initialFilesHS = !String.IsNullOrWhiteSpace(initialFiles) 
                                             ? new HashSet<string>(initialFiles.Split(new char[] { ',' })) 
                                             : new HashSet<string>();
            HashSet<string> ImagesToAdd = new HashSet<string>();
            HashSet<string> DocsToAdd = new HashSet<string>();

            var filesTodelet = initialFilesHS.Except(filesHS);
            foreach (var fileToDel in filesTodelet)
            {
                UploadHelper.deleteFile(fileToDel, folder);
            }

            var filesToInsert = filesHS.Except(initialFilesHS);
            foreach (var fileToInsert in filesToInsert)
            {
                UploadHelper.moveFile(fileToInsert, folder);
            }

            foreach (var file in filesHS)
            {
                if (!String.IsNullOrWhiteSpace(file))
                {
                    if (IsImage(file))
                    {
                        ImagesToAdd.Add(file);
                    }
                    else DocsToAdd.Add(file);
                }
            }
            Result.ImagesToUpload = ImagesToAdd.Count > 0 ?  String.Join(",", ImagesToAdd) : null;
            Result.DocsToUpload = DocsToAdd.Count > 0 ? String.Join(",", DocsToAdd) : null;
            return Result;
        }


        public static bool IsImage(string fileName)
        {
            string[] IMGExt = { "jpg", "png", "gif", "jpeg" };
            var fileNameToLower = fileName.ToLower();
            var ext = fileNameToLower.Substring(fileNameToLower.LastIndexOf('.') + 1);
            if (IMGExt.Contains(ext))
            {
                return true;
            }
            else return false;
        }
    }

    public class UploadedFilesString
    {
        public string ImagesToUpload { get; set; }
        public string DocsToUpload { get; set; }
    }
}