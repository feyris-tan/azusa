SELECT prod.id,
	   prod.name,
	   prod.price,
	   prod."boughtOn",
	   LENGTH(prod.screenshot) AS screenshotSize,
	   LENGTH(prod.picture) AS pictureSize,
	   prod.nsfw,
	   (SELECT media.mediaTypeId FROM azusa_media media WHERE media.relatedProduct = prod.id ORDER BY media.id ASC LIMIT 1) AS mediaTypeId,
	   (SELECT COUNT(*) FROM azusa_media WHERE azusa_media.relatedProduct = prod.id) AS numDiscs,
	   (SELECT COUNT(*) FROM azusa_media WHERE azusa_media.relatedProduct = prod.id AND((dumppath IS NULL) OR(dumppath = ''))) AS numUndumped,
	   (SELECT COUNT(*) FROM azusa_media m, azusa_mediaTypes mt WHERE m.relatedProduct = prod.id AND((m.graphData IS NULL) OR(m.graphData = '')) AND m.mediaTypeId = mt.id AND mt.graphData) AS numMissingGraph
FROM azusa_products prod
WHERE inShelf = @shelf
ORDER BY name ASC