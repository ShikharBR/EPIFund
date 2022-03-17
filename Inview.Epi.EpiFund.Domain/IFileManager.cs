using Inview.Epi.EpiFund.Domain.Enum;
using Inview.Epi.EpiFund.Domain.ViewModel;
using System;
using System.Drawing;
using System.Web;
using System.Web.Mvc;

namespace Inview.Epi.EpiFund.Domain
{
	public interface IFileManager
	{
		byte[] ConvertImageToBytes(Image img);

		void CreateThumbnail(string filename, Guid assetId);

		int CreateUserFile(HttpPostedFileBase file, int userId, UploadUserFileType? type);

		bool DeleteFile(string fileName, Guid assetId, FileType filetype);

		bool DeleteUserFile(int userFileId);

		FilePathResult DownloadDocument(string fileName, string assetId, string contentType, string fileDownloadName);

		string GetAssetImages();

		string GetFileSize(int byteSize);

		FilePathResult GetImageSource(string fileName, string assetId, string contentType);

		FilePathResult GetMainImageSource(Guid assetId);

		ThumbnailViewModel GetTempImageThumbnailByte(string filename, string assetId, string dateString, string userId, int thumbnailHeight, int thumbnailWidth);

		byte[] GetThumbnailByte(string filename, string assetId, int thumbnailHeight, int thumbnailWidth);

		byte[] GetViewAssetImage(string filename, string assetId, string contentType, int thumbnailHeight, int thumbnailWidth);

		string MoveTempAssetImage(string fileName, Guid assetId, string dateString, string userId);

		string SaveFile(HttpPostedFileBase file, Guid assetId, FileType filetype);

		bool SaveTempFile(HttpPostedFileBase file, Guid assetId, FileType fileType, string dateString, string userId);
	}
}