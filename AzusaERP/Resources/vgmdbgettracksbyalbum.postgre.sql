SELECT disc.name, disc.discindex, root.trackindex, root.name, track.tracklength, root.lang
FROM dump_vgmdb.album_disc_track_translation root
JOIN dump_vgmdb.album_discs disc ON disc.albumid = root.albumid AND disc.discindex = root.discindex
JOIN dump_vgmdb.album_disc_tracks track ON track.albumid = root.albumid AND track.discindex = root.discindex AND track.trackindex = root.trackindex
WHERE root.albumid = @id
ORDER BY root.albumid ASC, root.lang ASC, root.discindex ASC, root.trackindex ASC