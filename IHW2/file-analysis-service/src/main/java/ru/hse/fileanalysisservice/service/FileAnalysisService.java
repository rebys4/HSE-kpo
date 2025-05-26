package ru.hse.fileanalysisservice.service;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import org.springframework.core.io.Resource;
import ru.hse.fileanalysisservice.entity.FileAnalysisResult;
import ru.hse.fileanalysisservice.repository.FileAnalysisResultRepository;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.time.LocalDateTime;
import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class FileAnalysisService {

    private final FileAnalysisResultRepository repository;
    private final RestTemplate restTemplate = new RestTemplate();

    @Value("${file.storing.url}")
    private String fileStoringUrl;

    public FileAnalysisResult analyzeFile(Long fileId) {
        // 1. Получаем файл по id с file-storing-service
        String fileUrl = fileStoringUrl + "/files/" + fileId;
        Resource resource = restTemplate.getForObject(fileUrl, Resource.class);

        String content = null;
        String fileName = "unknown.txt";
        try (BufferedReader reader = new BufferedReader(
                new InputStreamReader(resource.getInputStream(), StandardCharsets.UTF_8))) {
            content = reader.lines().collect(Collectors.joining("\n"));
            fileName = resource.getFilename();
        } catch (Exception e) {
            throw new RuntimeException("Failed to read file from storing service", e);
        }

        // 2. Считаем статистику
        int paragraphs = content.split("(\\r?\\n){2,}").length;
        int words = Arrays.stream(content.split("\\s+")).filter(s -> !s.isBlank()).toArray().length;
        int chars = content.length();

        // 3. Собираем частотный словарь для word cloud
        Map<String, Integer> freq = new HashMap<>();
        for (String word : content.toLowerCase().replaceAll("[^a-zA-Zа-яА-Я0-9\\s]", "").split("\\s+")) {
            if (!word.isBlank()) freq.put(word, freq.getOrDefault(word, 0) + 1);
        }
        // 4. Формируем url для word cloud
        String wordsParam = freq.entrySet().stream()
                .sorted((a, b) -> -Integer.compare(a.getValue(), b.getValue()))
                .limit(50)
                .map(e -> e.getKey() + ":" + e.getValue())
                .collect(Collectors.joining(","));

        String wordCloudUrl = "https://quickchart.io/wordcloud?width=600&height=400&fontScale=15&scale=log&text=" +
                wordsParam.replace(":", "%20").replace(",", "%20");

        // 5. Сохраняем результат
        FileAnalysisResult result = FileAnalysisResult.builder()
                .fileId(fileId)
                .fileName(fileName)
                .paragraphsCount(paragraphs)
                .wordsCount(words)
                .charactersCount(chars)
                .wordCloudUrl(wordCloudUrl)
                .analyzedAt(LocalDateTime.now())
                .build();
        return repository.save(result);
    }

    public Optional<FileAnalysisResult> getAnalysis(Long fileId) {
        return repository.findByFileId(fileId);
    }
}