SELECT tag,
       (SELECT COUNT(*)
        FROM dump_gb_posttags posttag
                 LEFT JOIN dump_gb_posts post ON post.id = posttag.postid
        WHERE posttag.tagid = root.id AND post.downloaded AND post.rating != 'e')  AS numImages,
       id
FROM dump_gb_tags root
WHERE LENGTH(tag) > 1
ORDER BY tag ASC