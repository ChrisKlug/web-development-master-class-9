import { getFeaturedProducts } from '$lib/api/products/getFeaturedProducts';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ fetch }) => {
	return {
		title: 'Products',
		featured: await getFeaturedProducts(fetch)
	};
};
