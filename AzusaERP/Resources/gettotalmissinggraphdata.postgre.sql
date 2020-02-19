select COUNT(*)
from azusa.media media 
join azusa.products product on media.relatedProduct = product.id
join azusa.shelves shelf on product.inshelf = shelf.id
join azusa.mediatypes mediaType on media.mediatypeid = mediaType.id
where (
		((media.graphdata) IS NULL or (media.graphdata = ''))
	and (product.consistent = FALSE) 
	and (mediaType.graphData = TRUE)
	and ((mediaType.ignoreForStatistics = FALSE) 
	  or (shelf.ignoreForStatistics = FALSE))
		)
