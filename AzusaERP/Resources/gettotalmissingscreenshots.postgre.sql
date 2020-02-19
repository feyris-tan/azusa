select COUNT(*)
from azusa.products products
join azusa.shelves shelves on products.inshelf = shelves.id
where shelves.screenshotRequired = TRUE
and (products.screenshot IS NULL or LENGTH(products.screenshot) = 0)