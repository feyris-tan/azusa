SELECT id, name, mediaTypeId
FROM azusa_media 
WHERE relatedproduct=@productId 
ORDER BY id ASC