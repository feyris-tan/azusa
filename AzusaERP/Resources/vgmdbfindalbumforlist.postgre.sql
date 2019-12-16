SELECT
    root.id,
    root.catalog,
    root.release_date,
    type.name,
    classification.name,
    mediaformat.name,
    root.name,
    publishformat.name,
    root.notes,
    publisher.name

FROM dump_vgmdb_albums root
LEFT JOIN dump_vgmdb.album_types type ON root.typeid = type.id
LEFT JOIN dump_vgmdb.album_classification classification ON root.classificationid = classification.id
LEFT JOIN dump_vgmdb.album_mediaformat mediaformat ON root.mediaformatid = mediaformat.id
LEFT JOIN dump_vgmdb.album_mediaformat publishformat ON root.publishformatid = publishformat.id
LEFT JOIN dump_vgmdb.labels publisher ON root.publisherid = publisher.id

WHERE root.id=@id