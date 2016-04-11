using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace djfoxer.dp.notification.App.Model
{
    public class BaseStorageService
    {

        public void StorageLocalWrite(string fileName, object data)
        {
            StorageWrite(fileName, data);
        }

        public async Task<T> StorageLocalRead<T>(string fileName)
        {
            return await StorageRead<T>(fileName);
        }

        public void StorageRoamingWrite(string fileName, object data)
        {
            StorageWrite(fileName, data, false);
        }

        public async Task<T> StorageRoamingRead<T>(string fileName)
        {
            return await StorageRead<T>(fileName, false);
        }

        public void StorageRoamingDelete(string fileName)
        {
            StorageDelete(fileName,false);
        }

        public void StorageLocalDelete(string fileName)
        {
            StorageDelete(fileName);
        }

        private async void StorageWrite(string fileName, object data, bool local = true)
        {
            StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
                : ApplicationData.Current.RoamingFolder;
            StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(data));
        }

        private async void StorageDelete(string fileName, bool local = true)
        {
            try
            {
                StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
                : ApplicationData.Current.RoamingFolder;
                StorageFile file = await localFolder.GetFileAsync(fileName);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception)
            {

            }

        }

        private async Task<T> StorageRead<T>(string fileName, bool local = true)
        {
            try
            {
                StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
                : ApplicationData.Current.RoamingFolder;
                StorageFile file = await localFolder.GetFileAsync(fileName);

                string data = await FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<T>(data);

            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public string GetHashString(string inputString)
        {
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(inputString, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);
            return CryptographicBuffer.EncodeToHexString(buffHash);
        }



    }
}
