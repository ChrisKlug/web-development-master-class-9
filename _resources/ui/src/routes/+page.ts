import { getFeaturedProducts } from '$lib/api/products/getFeaturedProducts';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ fetch, parent }) => {
	const parentData = await parent();
	return {
		featured: await getFeaturedProducts(fetch),
		cart: parentData.cart
	};
};
