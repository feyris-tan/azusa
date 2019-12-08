SELECT label.name, role.name
FROM dump_vgmdb_album_labels root
JOIN dump_vgmdb_labels label ON root.labelid = label.id
JOIN dump_vgmdb_album_label_roles role ON root.roleid = role.id
WHERE albumid=@id