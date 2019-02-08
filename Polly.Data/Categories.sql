select c.Id, c.[Description], Count(*) [Count] from Product p 
inner join Productcategory pc on pc.ProductId = p.Id
inner join Category c on c.Id = pc.CategoryId
group by c.Id, c.[Description]
order by [Count] desc

select p.* from Product p 
inner join Productcategory pc on pc.ProductId = p.Id
inner join Category c on c.Id = pc.CategoryId
where pc.CategoryId = 1