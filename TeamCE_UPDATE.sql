--“numCheckins”
UPDATE Business
SET numCheckins = C.count FROM (
	SELECT business_id, COUNT(*)
	FROM Checkin
	GROUP BY business_id
	) AS C
WHERE Business.business_id = C.business_id;

--“reviewcount”
UPDATE Business
SET numCheckins = C.count FROM (
	SELECT business_id, COUNT(*)
	FROM Review
	GROUP BY business_id
	) AS C
WHERE Business.business_id = C.business_id;

--“reviewrating”
UPDATE Business
SET reviewrating = A.avg FROM (
	SELECT business_id, AVG(stars)
	FROM Review
	GROUP BY business_id
	) AS A
WHERE Business.business_id = A.business_id;

