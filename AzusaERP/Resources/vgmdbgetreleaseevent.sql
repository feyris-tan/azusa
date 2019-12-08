SELECT event.name
FROM dump_vgmdb_album_releaseevent root
JOIN dump_vgmdb_events event ON root.eventid = event.id
WHERE root.albumid = @albumid