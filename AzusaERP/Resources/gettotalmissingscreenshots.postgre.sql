select COUNT(*)
from azusa_products products
join azusa_shelves shelves on products.inshelf = shelves.id
where shelves."screenshotRequired" = TRUE
and (products.screenshot IS NULL or LENGTH(products.screenshot) = 0)