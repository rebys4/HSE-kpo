// controller/OrderController.java
package ru.hse.ordersservice.controller;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;
import ru.hse.ordersservice.entity.Order;
import ru.hse.ordersservice.repository.OrderRepository;
import ru.hse.ordersservice.service.OrderService;

import java.util.List;

@RestController
@RequestMapping("/orders")
@RequiredArgsConstructor
public class OrderController {
    private final OrderService orderService;
    private final OrderRepository orderRepository;

    @PostMapping
    public Order createOrder(@RequestBody CreateOrderRequest request) {
        return orderService.createOrder(request.userId, request.amount, request.description);
    }

    @GetMapping
    public List<Order> listOrders() {
        return orderRepository.findAll();
    }

    @GetMapping("/{id}")
    public Order getOrder(@PathVariable Long id) {
        return orderRepository.findById(id).orElseThrow();
    }

    public record CreateOrderRequest(Long userId, Integer amount, String description) {}
}