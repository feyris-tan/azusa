select count(*)
from azusa.products product
join azusa.shelves shelf on shelf.id = product.inshelf 
where 
	((product.picture) IS NULL 
     and (product.consistent = FALSE) 
     and (shelf.ignoreForStatistics = FALSE)
    )