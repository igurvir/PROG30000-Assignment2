# API ENDPOINTS CHECKLIST

Project Setup and API Integration

✅ Project Setup: Downloaded the starter project, added it to Git, and committed frequently.

✅ Imagga API Integration: Signed up for a free Imagga API, integrated it into the ImageHelper.GetTags method, and replaced the placeholders with actual API keys.

✅ Database Setup: Enabled database support using SQLite.

API Endpoints and Functionality


GET /api/images

✅ Returns paginated images (10 per page), ordered by posting date.

✅ Includes metadata (totalPages, totalImages) and pagination links.

GET /api/images/{id}

✅ Returns image details (ID, URL, username, user ID, tags).

✅ Returns 404 Not Found if the image does not exist and 400 Bad Request for invalid ID format.

GET /api/images/byTag

✅ Returns images filtered by the specified tag, paginated (10 per page), ordered by posting date.

✅ Returns 404 Not Found if no images match the given tag.

GET /api/images/populartags

✅ Returns the top 5 popular tags from the database, ordered by frequency.

POST /api/users

✅ Adds a new user to the database.

✅ Validates that the name is required and the email is unique.

POST /api/users/{id}/image

✅ Adds an image to the specified user's image list.

✅ Uses ImageHelper.GetTags to generate tags for the image.

✅ Returns the user’s details, including the last 10 images.

GET /api/users/{id}

✅ Returns a user object with the last 10 images.

GET /api/users/{id}/images

✅ Returns all images for a specified user, paginated (10 per page), ordered by posting date.

DELETE /api/users/{id}

✅ Removes the specified user and all associated images.

Error Handling

✅ Ensures appropriate error responses for 404 Not Found, 400 Bad Request, etc.

✅ Provides detailed error messages as per the assignment format.
