SELECT figure.id, root.name, category.name, figure.barcode, figure.name, figure.release_date, figure.price, figurephoto.thumbnail
FROM dump_myfigurecollection.figures figure
LEFT JOIN dump_myfigurecollection.roots root ON figure.rootid = root.id
LEFT JOIN dump_myfigurecollection.categories category ON figure.categoryid = category.id
LEFT JOIN dump_myfigurecollection.figurephotos figurephoto ON figure.id = figurephoto.id
WHERE (figure.name LIKE @query
OR figure.barcode LIKE @query)
AND figure.enabled = TRUE