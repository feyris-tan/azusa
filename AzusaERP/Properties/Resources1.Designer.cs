﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace moe.yo3explorer.azusa.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("moe.yo3explorer.azusa.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap accept {
            get {
                object obj = ResourceManager.GetObject("accept", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap add {
            get {
                object obj = ResourceManager.GetObject("add", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap Find_VS {
            get {
                object obj = ResourceManager.GetObject("Find_VS", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT tag,
        ///       (SELECT COUNT(*)
        ///        FROM dump_gb.posttags posttag
        ///                 LEFT JOIN dump_gb.posts post ON post.id = posttag.postid
        ///        WHERE posttag.tagid = root.id AND post.downloaded AND post.rating != &apos;e&apos;)  AS numImages,
        ///       id
        ///FROM dump_gb.tags root
        ///WHERE LENGTH(tag) &gt; 1
        ///ORDER BY tag ASC ähnelt.
        /// </summary>
        internal static string Gelbooru_GetTags_Postgre {
            get {
                return ResourceManager.GetString("Gelbooru_GetTags_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT DISTINCT t.tag, 1, pt.tagid
        ///FROM dump_gb.posttags pt
        ///LEFT JOIN dump_gb.tags t ON t.id; ähnelt.
        /// </summary>
        internal static string Gelbooru_GetTags_SQLite {
            get {
                return ResourceManager.GetString("Gelbooru_GetTags_SQLite", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT id, name, mediaTypeId
        ///FROM azusa.media 
        ///WHERE relatedproduct=@productId 
        ///ORDER BY id ASC ähnelt.
        /// </summary>
        internal static string GetMediaByProduct_Postgre {
            get {
                return ResourceManager.GetString("GetMediaByProduct_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT prod.id,
        ///	   prod.name,
        ///	   prod.price,
        ///	   prod.&quot;boughtOn&quot;,
        ///	   LENGTH(prod.screenshot) AS screenshotSize,
        ///	   LENGTH(prod.picture) AS pictureSize,
        ///	   prod.nsfw,
        ///	   (SELECT media.mediaTypeId FROM azusa.media media WHERE media.relatedProduct = prod.id ORDER BY media.id ASC LIMIT 1) AS mediaTypeId,
        ///	   (SELECT COUNT(*) FROM azusa.media WHERE azusa.media.relatedProduct = prod.id) AS numDiscs,
        ///	   (SELECT COUNT(*) FROM azusa.media WHERE azusa.media.relatedProduct = prod.id AND((dumppath IS NU [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string GetProductsInShelf_Postgre {
            get {
                return ResourceManager.GetString("GetProductsInShelf_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die select count(*)
        ///from azusa.products product
        ///join azusa.shelves shelf on shelf.id = product.inshelf 
        ///where 
        ///	((product.picture) IS NULL 
        ///     and (product.consistent = FALSE) 
        ///     and (shelf.&quot;ignoreForStatistics&quot; = FALSE)
        ///    ) ähnelt.
        /// </summary>
        internal static string GetTotalMissingCovers_Postgre {
            get {
                return ResourceManager.GetString("GetTotalMissingCovers_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die select COUNT(*)
        ///from azusa.media media 
        ///join azusa.products product on media.relatedProduct = product.id
        ///join azusa.shelves shelf on product.inshelf = shelf.id
        ///join azusa.mediatypes mediaType on media.mediatypeid = mediaType.id
        ///where (
        ///		((media.graphdata) IS NULL or (media.graphdata = &apos;&apos;))
        ///	and (product.consistent = FALSE) 
        ///	and (mediaType.graphData = TRUE)
        ///	and ((mediaType.&quot;ignoreForStatistics&quot; = FALSE) 
        ///	  or (shelf.&quot;ignoreForStatistics&quot; = FALSE))
        ///		)
        /// ähnelt.
        /// </summary>
        internal static string GetTotalMissingGraphData_Postgre {
            get {
                return ResourceManager.GetString("GetTotalMissingGraphData_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die select COUNT(*)
        ///from azusa.products products
        ///join azusa.shelves shelves on products.inshelf = shelves.id
        ///where shelves.&quot;screenshotRequired&quot; = TRUE
        ///and (products.screenshot IS NULL or LENGTH(products.screenshot) = 0) ähnelt.
        /// </summary>
        internal static string GetTotalMissingScreenshots_Postgre {
            get {
                return ResourceManager.GetString("GetTotalMissingScreenshots_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die select count(*) 
        ///from azusa.media media 
        ///left join azusa.products products on media.relatedproduct = products.id 
        ///left join azusa.platforms platforms on products.platform = platforms.id 
        ///left join azusa.shelves shelves on products.inShelf = shelves.id 
        ///left join azusa.mediatypes on media.mediaTypeId = azusa.mediatypes.id 
        ///where (media.dumppath IS NULL or media.dumppath = &apos;&apos;) 
        ///  and products.consistent = FALSE
        ///  and products.name IS NOT NULL
        ///  and azusa.mediatypes.&quot;ignoreForStatistics&quot; = FALSE ähnelt.
        /// </summary>
        internal static string GetTotalUndumpedMedia_Postgre {
            get {
                return ResourceManager.GetString("GetTotalUndumpedMedia_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap internet_web_browser {
            get {
                object obj = ResourceManager.GetObject("internet_web_browser", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap media_optical {
            get {
                object obj = ResourceManager.GetObject("media_optical", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT figure.id, root.name, category.name, figure.barcode, figure.name, figure.release_date, figure.price, figurephoto.thumbnail
        ///FROM dump_myfigurecollection.figures figure
        ///LEFT JOIN dump_myfigurecollection.roots root ON figure.rootid = root.id
        ///LEFT JOIN dump_myfigurecollection.categories category ON figure.categoryid = category.id
        ///LEFT JOIN dump_myfigurecollection.figurephotos figurephoto ON figure.id = figurephoto.id
        ///WHERE (figure.name LIKE @query
        ///OR figure.barcode LIKE @query)
        ///AND figure.enabled  [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string MyFigureCollectionSearch_Postgre {
            get {
                return ResourceManager.GetString("MyFigureCollectionSearch_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap NavBack {
            get {
                object obj = ResourceManager.GetObject("NavBack", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap NavForward {
            get {
                object obj = ResourceManager.GetObject("NavForward", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap openfolderHS {
            get {
                object obj = ResourceManager.GetObject("openfolderHS", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT game.platform, game.sku,game.title,game.languages,game.commontitle,game.region, genre.name, 
        ///	   developer.name, publisher.name, game.daterelease, game.cover, game.description, game.barcode
        ///FROM dump_psxdatacenter.games game
        ///JOIN dump_psxdatacenter.genres genre ON game.genreid = genre.id
        ///JOIN dump_psxdatacenter.companies developer ON game.developerid = developer.id
        ///JOIN dump_psxdatacenter.companies publisher ON game.publisherid = publisher.id
        ///WHERE game.id=@id ähnelt.
        /// </summary>
        internal static string PsxDatacenterGetGame_Postgre {
            get {
                return ResourceManager.GetString("PsxDatacenterGetGame_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap RefreshDocViewHS {
            get {
                object obj = ResourceManager.GetObject("RefreshDocViewHS", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT
        ///    root.id,
        ///    root.catalog,
        ///    root.release_date,
        ///    type.name,
        ///    classification.name,
        ///    mediaformat.name,
        ///    root.name,
        ///    publishformat.name,
        ///    root.notes,
        ///    publisher.name
        ///
        ///FROM dump_vgmdb_albums root
        ///LEFT JOIN dump_vgmdb.album_types type ON root.typeid = type.id
        ///LEFT JOIN dump_vgmdb.album_classification classification ON root.classificationid = classification.id
        ///LEFT JOIN dump_vgmdb.album_mediaformat mediaformat ON root.mediaformatid = mediaformat.id
        ///LEFT JOIN dump_ [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string VgmDbFindAlbumForList_Postgre {
            get {
                return ResourceManager.GetString("VgmDbFindAlbumForList_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT root.name, role.name
        ///FROM dump_vgmdb.album_label_arbiturary root
        ///JOIN dump_vgmdb.album_label_roles role ON root.roleid = role.id
        ///WHERE root.albumid=@id ähnelt.
        /// </summary>
        internal static string VgmdbFindArbituraryLabelNamesByAlbumId_Postgre {
            get {
                return ResourceManager.GetString("VgmdbFindArbituraryLabelNamesByAlbumId_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT label.name, role.name
        ///FROM dump_vgmdb.album_labels root
        ///JOIN dump_vgmdb.labels label ON root.labelid = label.id
        ///JOIN dump_vgmdb.album_label_roles role ON root.roleid = role.id
        ///WHERE albumid=@id ähnelt.
        /// </summary>
        internal static string VgmdbFindLabelsByAlbumId_Postgre {
            get {
                return ResourceManager.GetString("VgmdbFindLabelsByAlbumId_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT root.name, type.name
        ///FROM dump_vgmdb.album_artist_arbitrary root
        ///JOIN dump_vgmdb.album_artist_type type ON root.artisttypeid = type.id
        ///WHERE albumid = @id; ähnelt.
        /// </summary>
        internal static string VgmdbGetArbitraryArtistsByAlbumId_Postgre {
            get {
                return ResourceManager.GetString("VgmdbGetArbitraryArtistsByAlbumId_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT artist.name, type.name
        ///FROM dump_vgmdb.album_artists root
        ///LEFT JOIN dump_vgmdb.artist artist ON root.artistid = artist.id
        ///LEFT JOIN dump_vgmdb.album_artist_type type ON root.artisttypeid = type.id
        ///WHERE root.albumid = @id; ähnelt.
        /// </summary>
        internal static string VgmdbGetArtistNamesByAlbumId_Postgre {
            get {
                return ResourceManager.GetString("VgmdbGetArtistNamesByAlbumId_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT  album.catalog, 
        ///		album.name
        ///FROM dump_vgmdb.album_relatedalbum root
        ///JOIN dump_vgmdb.albums album ON root.relatedalbumid = album.id
        ///WHERE root.albumid = @albumid ähnelt.
        /// </summary>
        internal static string VgmdbGetRelatedAlbums_Postgre {
            get {
                return ResourceManager.GetString("VgmdbGetRelatedAlbums_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT event.name
        ///FROM dump_vgmdb.album_releaseevent root
        ///JOIN dump_vgmdb.events event ON root.eventid = event.id
        ///WHERE root.albumid = @albumid ähnelt.
        /// </summary>
        internal static string VgmDbGetReleaseEvent {
            get {
                return ResourceManager.GetString("VgmDbGetReleaseEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT albums.catalog,
        ///       albums.name
        ///FROM dump_vgmdb.album_reprints root
        ///JOIN dump_vgmdb.albums albums ON root.reprintid = albums.id
        ///WHERE root.albumid = @albumid ähnelt.
        /// </summary>
        internal static string VgmdbGetReprints_Postgre {
            get {
                return ResourceManager.GetString("VgmdbGetReprints_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT disc.name, disc.discindex, root.trackindex, root.name, track.tracklength, root.lang
        ///FROM dump_vgmdb.album_disc_track_translation root
        ///JOIN dump_vgmdb.album_discs disc ON disc.albumid = root.albumid AND disc.discindex = root.discindex
        ///JOIN dump_vgmdb.album_disc_tracks track ON track.albumid = root.albumid AND track.discindex = root.discindex AND track.trackindex = root.trackindex
        ///WHERE root.albumid = @id
        ///ORDER BY root.albumid ASC, root.lang ASC, root.discindex ASC, root.trackindex ASC ähnelt.
        /// </summary>
        internal static string VgmdbGetTracksByAlbum_Postgre {
            get {
                return ResourceManager.GetString("VgmdbGetTracksByAlbum_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT DISTINCT root.rid, release.title, release.gtin, release.catalog
        ///FROM dump_vndb.release_vns root
        ///JOIN dump_vndb.release release ON root.rid = release.id
        ///WHERE root.title LIKE @query
        ///   OR root.original LIKE @query
        ///   OR release.title LIKE @query
        ///   OR release.original LIKE @query
        ///   OR release.gtin LIKE @query
        ///   OR release.catalog LIKE @query ähnelt.
        /// </summary>
        internal static string VndbSearch_Postgre {
            get {
                return ResourceManager.GetString("VndbSearch_Postgre", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT album.name
        ///FROM dump_vocadb.albumtracks root
        ///JOIN dump_vocadb.albums album ON root.albumid = album.id
        ///WHERE lower(root.name) like lower(@query) ähnelt.
        /// </summary>
        internal static string Vocadb_FindSongRelatedAlbum {
            get {
                return ResourceManager.GetString("Vocadb_FindSongRelatedAlbum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die SELECT id, name, artiststring, disctype, releasedate, catalognumber
        ///FROM dump_vocadb.albums
        ///WHERE LOWER(name) LIKE LOWER(@query)
        ///OR LOWER(artiststring) LIKE LOWER(@query)
        ///OR LOWER(catalognumber) LIKE LOWER(@query) ähnelt.
        /// </summary>
        internal static string Vocadb_Search_Postgre {
            get {
                return ResourceManager.GetString("Vocadb_Search_Postgre", resourceCulture);
            }
        }
    }
}
