import type { AddOrderPayload } from '$lib/types/Order';
import { error } from '@sveltejs/kit';

export async function addOrder(client: typeof fetch, payload: AddOrderPayload) {
	const response = await client(`/api/orders`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify(payload)
	});

	if (response.ok) {
		return await response.json();
	} else {
		error(response.status, 'Failed to add order.');
	}
}
