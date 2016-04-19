using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace djfoxer.dp.notification.Core.Logic
{
    public class BaseStorageService
    {
        //private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        //public readonly Guid g = Guid.NewGuid();

        //private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public BaseStorageService()
        {

        }

        public void StorageLocalWrite(string fileName, object data)
        {
            StorageWrite(fileName, data);
        }

        public T StorageLocalRead<T>(string fileName)
        {
            return StorageRead<T>(fileName);
        }

        public void StorageRoamingWrite(string fileName, object data)
        {
            StorageWrite(fileName, data, false);
        }

        public T StorageRoamingRead<T>(string fileName)
        {
            return StorageRead<T>(fileName, false);
        }

        public void StorageRoamingDelete(string fileName)
        {
            StorageDelete(fileName, false);
        }

        public void StorageLocalDelete(string fileName)
        {
            StorageDelete(fileName);
        }

        private void StorageWrite(string fileName, object data, bool local = true)
        {
            ApplicationData.Current.LocalSettings.Values[fileName] = JsonConvert.SerializeObject(data);
            //  _readWriteLock.EnterWriteLock();
            //try
            //{

            //    StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
            //   : ApplicationData.Current.RoamingFolder;
            //    var check = await localFolder.TryGetItemAsync(fileName);
            //    StorageFile file = null;

            //    if (check == null)
            //    {
            //        file = await localFolder.CreateFileAsync(fileName);
            //    }
            //    else
            //    {
            //        file = await localFolder.GetFileAsync(fileName);
            //    }



            //    using (var transaction = await file.OpenTransactedWriteAsync())
            //    {
            //        using (DataWriter dataWriter = new DataWriter(transaction.Stream))
            //        {
            //            dataWriter.WriteString(JsonConvert.SerializeObject(data));
            //            transaction.Stream.Size = await dataWriter.StoreAsync(); // reset stream size to override the file
            //            await dataWriter.FlushAsync();
            //            await transaction.Stream.FlushAsync();

            //            await transaction.CommitAsync();
            //        }
            //    }

            //    return;


            //  }
            //catch (Exception)
            //{


            //}
            //finally
            //{

            //     //    _readWriteLock.ExitWriteLock();
            //}

        }

        private void StorageDelete(string fileName, bool local = true)
        {
            ApplicationData.Current.LocalSettings.Values[fileName] = null;
            //return;
            //try
            //{
            //    StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
            //    : ApplicationData.Current.RoamingFolder;
            //    StorageFile file = await localFolder.GetFileAsync(fileName);
            //    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            //}
            //catch (Exception)
            //{

            //}

        }

        private T StorageRead<T>(string fileName, bool local = true)
        {
            //   _readWriteLock.EnterReadLock();
            var obj = ApplicationData.Current.LocalSettings.Values[fileName] as string;

            if (obj != null)
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }

            return default(T); ;
            //try
            //{
            //    StorageFolder localFolder = local ? ApplicationData.Current.LocalFolder
            //    : ApplicationData.Current.RoamingFolder;
            //    StorageFile file = await localFolder.GetFileAsync(fileName);
            //    string data = string.Empty;
            //    using (var transaction = await file.OpenSequentialReadAsync())
            //    {
            //        using (DataReader dataReader = new DataReader(transaction))
            //        {
            //            var size = (uint)((FileRandomAccessStream)transaction).Size;
            //            await dataReader.LoadAsync(size);

            //            data = dataReader.ReadString(size);

            //            //  var buffer = new byte[((FileRandomAccessStream)transaction).Size];

            //            //        await transaction.ReadAsync(buffer, 0, InputStreamOptions.ReadAhead);
            //            //dataReader.ReadString(transaction.)
            //            //dataReader.InputStreamOptions = InputStreamOptions.ReadAhead;



            //            dataReader.DetachStream();

            //        }
            //    }


            //    //    string data = await FileIO.ReadTextAsync(file);
            //    return JsonConvert.DeserializeObject<T>(data);

            //}
            //catch (Exception)
            //{
            //    return default(T);
            //}
            //finally
            //{
            //    //       if (_readWriteLock.IsReadLockHeld)
            //    //            _readWriteLock.ExitReadLock();
            //}
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
