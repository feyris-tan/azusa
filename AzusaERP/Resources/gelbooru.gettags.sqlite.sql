SELECT DISTINCT t.tag, 1, pt.tagid
FROM dump_gb_posttags pt
LEFT JOIN dump_gb_tags t ON t.id;