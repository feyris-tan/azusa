SELECT id, name, mediaTypeId
FROM azusa.media 
WHERE relatedproduct=@productId 
ORDER BY id ASC