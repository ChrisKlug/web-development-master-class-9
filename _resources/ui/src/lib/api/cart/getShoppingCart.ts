import type { ShoppingCartItem } from '$lib/types/ShoppingCart';
import { error } from '@sveltejs/kit';

export async function getShoppingCart(client: typeof fetch): Promise<ShoppingCartItem[]> {
	const response = await client('/api/shopping-cart');
	if (response.ok) {
		return await response.json();
	} else {
		error(response.status, 'Could not fetch shopping cart');
	}
}
