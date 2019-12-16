select count(*) 
from azusa.media media 
left join azusa.products products on media.relatedproduct = products.id 
left join azusa.platforms platforms on products.platform = platforms.id 
left join azusa.shelves shelves on products.inShelf = shelves.id 
left join azusa.mediatypes on media.mediaTypeId = azusa.mediatypes.id 
where (media.dumppath IS NULL or media.dumppath = '') 
  and products.consistent = FALSE
  and products.name IS NOT NULL
  and azusa.mediatypes."ignoreForStatistics" = FALSE