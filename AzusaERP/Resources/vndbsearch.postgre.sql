SELECT DISTINCT root.rid, release.title, release.gtin, release.catalog
FROM dump_vndb.release_vns root
JOIN dump_vndb.release release ON root.rid = release.id
WHERE root.title LIKE @query
   OR root.original LIKE @query
   OR release.title LIKE @query
   OR release.original LIKE @query
   OR release.gtin LIKE @query
   OR release.catalog LIKE @query