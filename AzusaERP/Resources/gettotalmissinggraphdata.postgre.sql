select COUNT(*)
from azusa_media media 
join azusa_products product on media.relatedProduct = product.id
join azusa_shelves shelf on product.inshelf = shelf.id
join azusa_mediatypes mediaType on media.mediatypeid = mediaType.id
where (
		((media.graphdata) IS NULL or (media.graphdata = ''))
	and (product.consistent = FALSE) 
	and (mediaType.graphData = TRUE)
	and ((mediaType."ignoreForStatistics" = FALSE) 
	  or (shelf."ignoreForStatistics" = FALSE))
		)
