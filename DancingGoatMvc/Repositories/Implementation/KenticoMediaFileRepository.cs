using System;
using System.Collections.Generic;
using System.Linq;

using CMS.MediaLibrary;
using CMS.SiteProvider;

namespace DancingGoat.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of media files.
    /// </summary>
    public class KenticoMediaFileRepository : IMediaFileRepository
    {
        /// <summary>
        /// Returns instance of <see cref="MediaFileInfo"/> specified by library name.
        /// </summary>
        public MediaLibraryInfo GetByName(string mediaLibraryName)
        {
            return MediaLibraryInfoProvider.GetMediaLibraryInfo(mediaLibraryName, SiteContext.CurrentSiteName);
        }


        /// <summary>
        /// Returns all media files in the media library.
        /// </summary>
        public IEnumerable<MediaFileInfo> GetMediaFiles(string mediaLibraryName)
        {
            var mediaLibrary = GetByName(mediaLibraryName);

            if (mediaLibrary == null)
            {
                throw new ArgumentException("Media library not found.", nameof(mediaLibraryName));
            }

            return MediaFileInfoProvider.GetMediaFiles()
                .WhereEquals("FileLibraryID", mediaLibrary.LibraryID)
                .ToList();
        }
    }
}