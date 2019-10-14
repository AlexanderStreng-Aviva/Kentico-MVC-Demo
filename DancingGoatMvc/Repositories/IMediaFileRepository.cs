using System.Collections.Generic;

using CMS.MediaLibrary;

namespace DancingGoat.Repositories
{
    /// <summary>
    /// Represents a contract for a collection of media files.
    /// </summary>
    public interface IMediaFileRepository : IRepository
    {
        /// <summary>
        /// Returns a media library with the specified identifier. 
        /// </summary>
        /// <param name="mediaLibraryName">Media library identifier.</param>
        MediaLibraryInfo GetByName(string mediaLibraryName);


        /// <summary>
        /// Returns all media files in specified media library.
        /// </summary>
        /// <param name="mediaLibraryName">Media library identifier.</param>
        IEnumerable<MediaFileInfo> GetMediaFiles(string mediaLibraryName);
    }
}