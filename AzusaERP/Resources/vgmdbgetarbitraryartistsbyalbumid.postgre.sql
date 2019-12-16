SELECT root.name, type.name
FROM dump_vgmdb.album_artist_arbitrary root
JOIN dump_vgmdb.album_artist_type type ON root.artisttypeid = type.id
WHERE albumid = @id;