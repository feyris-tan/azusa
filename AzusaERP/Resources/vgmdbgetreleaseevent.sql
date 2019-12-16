SELECT event.name
FROM dump_vgmdb.album_releaseevent root
JOIN dump_vgmdb.events event ON root.eventid = event.id
WHERE root.albumid = @albumid