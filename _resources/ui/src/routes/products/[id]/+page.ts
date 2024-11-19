import { getFeaturedProducts } from '$lib/api/products/getFeaturedProducts';
import { getProduct } from '$lib/api/products/getProduct';
import type { PageLoad } from './$types';

export const load: PageLoad = async ({ params, fetch }) => {
	const { id } = params;
	const product = await getProduct(fetch, Number(id));

	return {
		title: 'Products',
		product: product,
		featured: getFeaturedProducts(fetch)
	};
};
