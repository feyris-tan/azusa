SELECT root.name, role.name
FROM dump_vgmdb.album_label_arbiturary root
JOIN dump_vgmdb.album_label_roles role ON root.roleid = role.id
WHERE root.albumid=@id