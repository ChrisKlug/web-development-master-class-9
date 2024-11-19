import type { Product } from '$lib/types/Product';
import { error } from '@sveltejs/kit';

export async function getFeaturedProducts(client: typeof fetch): Promise<Product[]> {
	const response = await client('/api/products/featured');
	if (response.ok) {
		return await response.json();
	} else {
		error(response.status, 'Failed to get featured products');
	}
}
