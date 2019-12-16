SELECT label.name, role.name
FROM dump_vgmdb.album_labels root
JOIN dump_vgmdb.labels label ON root.labelid = label.id
JOIN dump_vgmdb.album_label_roles role ON root.roleid = role.id
WHERE albumid=@id