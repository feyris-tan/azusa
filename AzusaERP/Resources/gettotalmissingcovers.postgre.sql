select count(*)
from azusa_products product
join azusa_shelves shelf on shelf.id = product.inshelf 
where 
	((product.picture) IS NULL 
     and (product.consistent = FALSE) 
     and (shelf."ignoreForStatistics" = FALSE)
    )