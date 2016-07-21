using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;
using System.Runtime.Serialization;

namespace Alias.Utils
{
    public class SerializationManager
    {
        private string _fileName;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public SerializationManager(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Сохранить объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<bool> Save<T>(T data)
        {
            bool result = true;
            try
            {
                MemoryStream sessionData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(sessionData, data);

                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_fileName, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Восстановить объект
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> Restore<T>()
        {
            T data = default(T);
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(_fileName);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    data = (T)serializer.ReadObject(inStream.AsStreamForRead());
                }
            }
            catch { }
            return data;
        }

        /// <summary>
        /// Удалить сохранение
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Delete()
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(_fileName);
                await file.DeleteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
