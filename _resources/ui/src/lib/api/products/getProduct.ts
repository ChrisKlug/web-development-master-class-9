import type { Product } from '$lib/types/Product';
import { error } from '@sveltejs/kit';

export async function getProduct(client: typeof fetch, id: number): Promise<Product> {
	const response = await client(`/api/products/${id}`);
	if (response.ok) {
		return await response.json();
	} else {
		error(response.status, 'Failed to fetch product');
	}
}
