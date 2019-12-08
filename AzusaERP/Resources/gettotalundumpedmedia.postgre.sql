select count(*) 
from azusa_media media 
left join azusa_products products on media.relatedproduct = products.id 
left join azusa_platforms platforms on products.platform = platforms.id 
left join azusa_shelves shelves on products.inShelf = shelves.id 
left join azusa_mediatypes on media.mediaTypeId = azusa_mediatypes.id 
where (media.dumppath IS NULL or media.dumppath = '') 
  and products.consistent = FALSE
  and products.name IS NOT NULL
  and azusa_mediatypes."ignoreForStatistics" = FALSE