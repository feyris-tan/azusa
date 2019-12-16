SELECT DISTINCT t.tag, 1, pt.tagid
FROM dump_gb.posttags pt
LEFT JOIN dump_gb.tags t ON t.id;