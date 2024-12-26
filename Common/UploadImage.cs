using Microsoft.Extensions.FileProviders;

namespace E_Recrutement.Common
{
	public class UploadImage
	{
		public static string UploadImageFile(IFormFile files, string path)
		{
			if (files != null)
			{
				if (files.Length > 0)
				{
					var fileName = Path.GetFileName(files.FileName);
					var myUniqueFileName = Convert.ToString(Guid.NewGuid());
					var fileExtension = Path.GetExtension(fileName);
					var newFileName = string.Concat(myUniqueFileName, fileExtension);

					var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path)).Root
						+ $@"\{newFileName}";

					using (FileStream fs = System.IO.File.Create(filepath))
					{
						files.CopyTo(fs);
						fs.Flush();
					}
					var newImageName = newFileName;
					return newImageName.ToString();
				}
				else
				{
					return String.Empty;
				}
			}
			else
			{
				return String.Empty;
			}
		}
	}
}
