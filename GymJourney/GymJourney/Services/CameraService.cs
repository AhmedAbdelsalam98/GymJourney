using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GymJourney.Services
{
    //=============================================
    // Reference A3: externally sourced algorithm
    // Purpose: This class has methods that take advantage of camera service plugins
    // Date: 8/11/2020
    // Source: The step by step set up of Media Plugin for Xamarin.Forms!
    // Author: Udara Alwis
    // url: https://theconfuzedsourcecode.wordpress.com/2020/01/28/the-step-by-step-set-up-of-media-plugin-for-xamarin-forms/
    // Adaptation: updated obsolete methods and changed takephoto function to return filepath
    //=============================================
    class CameraService
    {
        public async Task<string> TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable ||
                    !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var isPermissionGranted = await RequestCameraAndGalleryPermissions();
            if (!isPermissionGranted)
                return null;

            var file = await CrossMedia.Current.TakePhotoAsync(new
                Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "GymJournal",
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Medium,
            });

            if (file == null)
                return null;

            return file.Path;
        }

        private async Task<bool> RequestCameraAndGalleryPermissions()
        {   
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();

            if (
            cameraStatus != PermissionStatus.Granted ||
            storageStatus != PermissionStatus.Granted ||
            photosStatus != PermissionStatus.Granted)
            {
                var permissions = new Permission[]
                    {
                Permission.Camera,
                Permission.Storage,
                Permission.Photos
                };
                var cpermissionRequestResult = await CrossPermissions.Current.
                RequestPermissionAsync<CameraPermission>();
                var spermissionRequestResult = await CrossPermissions.Current.
                RequestPermissionAsync<StoragePermission>();
                var ppermissionRequestResult = await CrossPermissions.Current.
                RequestPermissionAsync<PhotosPermission>();

                var cameraResult = cpermissionRequestResult;
                var storageResult = spermissionRequestResult;
                var photosResults = ppermissionRequestResult;

                return (
                    cameraResult != PermissionStatus.Denied &&
                    storageResult != PermissionStatus.Denied &&
                    photosResults != PermissionStatus.Denied);
            }

            return true;
        }
    }
    //=============================================
    // End reference A3
    //=============================================
}
