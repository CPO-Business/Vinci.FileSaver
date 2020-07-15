using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Vinci.FileSaver.Interface
{
    public interface IStorage
    {
        /// <summary>
        /// 保存图片，如果要自定义保存文件名称
        /// </summary>
        /// <param name="img"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath">用户自定义的文件名称和路径，将没有系统默认的文件夹</param>
        /// <returns>id</returns>
        string SaveImage(Image img, string extension, DirectoryInfo rootDir = null, string filePath = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath">id or Path: id 的格式为："id:***",path 的格式为:"path:***",若无前缀标识则为id</param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        Image GetImage(string idOrPath, DirectoryInfo rootDir = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileToSave">将要保存的文件流</param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string SaveFile(Stream fileToSave, string extension, DirectoryInfo rootDir = null, string filePath = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileSaveAct">Action<FileStream> 将创建文件流传递给方法，用户需要在方法中自行写入文件内容，注意不要释放文件流，系统会自动完成所有动作 </param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string SaveFile(Action<FileStream> fileSaveAct, string extension, DirectoryInfo rootDir = null, string filePath = null);

        /// <summary>
        /// return: FileStream 需要用户自己释放, return null if file cannot be found
        /// </summary>
        /// <param name="idOrPath"></param>
        /// <param name="rootDir"></param>
        /// <returns>FileStream 需要用户自己释放, return null if file cannot be found</returns>
        FileStream GetFile(string idOrPath, DirectoryInfo rootDir = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath"></param>
        /// <param name="rootDir"></param>
        /// <param name="newIdOrPath">修改路径或名称，功能相当于重命名</param>
        /// <returns></returns>
        bool MoveFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null);

        bool CopyFile(string idOrPath, string newPath, DirectoryInfo rootDir = null, DirectoryInfo newRootDir = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrPath">id or Path: id 的格式为："id:***",path 的格式为:"path:***",若无前缀标识则为id</param>
        /// <param name="img"></param>
        /// <param name="extension"></param>
        /// <param name="rootDir"></param>
        /// <returns></returns>
        bool UpdateImage(string idOrPath, Image img, string extension, DirectoryInfo rootDir = null);
    }
}
