CREATE OR REPLACE FUNCTION update_reviewcount_reviewrating() RETURNS TRIGGER AS '
BEGIN
	UPDATE Business
	SET review_count = review_count + 1, reviewrating = (Business.reviewrating + NEW.stars / Business.review_count)
	WHERE Business.business_id = NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_numCheckins() RETURNS TRIGGER AS '
BEGIN
	UPDATE Business
	SET numcheckins = numcheckins + 1
	WHERE Business.business_id = NEW.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;


CREATE TRIGGER NewReview
	AFTER INSERT ON Review
	FOR EACH ROW
	EXECUTE PROCEDURE update_reviewcount_reviewrating(); 
	
CREATE TRIGGER NewCheckin
	AFTER INSERT ON Checkin
	FOR EACH ROW
	EXECUTE PROCEDURE update_numCheckins();
