import { error } from '@sveltejs/kit';

export async function addToCart(client: typeof fetch, productId: number, count?: number) {
	const response = await client(`/api/shopping-cart`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify({ productId, count: count ?? 1 })
	});

	if (response.ok) {
		return await response.json();
	} else {
		error(response.status, 'Failed to add to cart.');
	}
}
