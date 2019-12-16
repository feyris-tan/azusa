SELECT artist.name, type.name
FROM dump_vgmdb.album_artists root
LEFT JOIN dump_vgmdb.artist artist ON root.artistid = artist.id
LEFT JOIN dump_vgmdb.album_artist_type type ON root.artisttypeid = type.id
WHERE root.albumid = @id;