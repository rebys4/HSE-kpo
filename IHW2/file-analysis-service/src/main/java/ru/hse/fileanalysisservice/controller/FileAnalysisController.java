package ru.hse.fileanalysisservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hse.fileanalysisservice.entity.FileAnalysisResult;
import ru.hse.fileanalysisservice.service.FileAnalysisService;

import java.util.Optional;

@RestController
@RequestMapping("/analyse")
@RequiredArgsConstructor
public class FileAnalysisController {
    private final FileAnalysisService analysisService;

    @Operation(summary = "Проанализировать файл по id")
    @PostMapping("/{fileId}")
    public ResponseEntity<FileAnalysisResult> analyzeFile(@PathVariable Long fileId) {
        try {
            FileAnalysisResult result = analysisService.analyzeFile(fileId);
            return ResponseEntity.ok(result);
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseEntity.internalServerError().build();
        }
    }

    @Operation(summary = "Получить результаты анализа по id файла")
    @GetMapping("/{fileId}")
    public ResponseEntity<FileAnalysisResult> getAnalysis(@PathVariable Long fileId) {
        Optional<FileAnalysisResult> result = analysisService.getAnalysis(fileId);
        return result.map(ResponseEntity::ok)
                .orElseGet(() -> ResponseEntity.notFound().build());
    }

    @Operation(summary = "Получить ссылку на облако слов")
    @GetMapping("/{fileId}/wordcloud")
    public ResponseEntity<String> getWordCloud(@PathVariable Long fileId) {
        Optional<FileAnalysisResult> result = analysisService.getAnalysis(fileId);
        return result.map(r -> ResponseEntity.ok(r.getWordCloudUrl()))
                .orElseGet(() -> ResponseEntity.notFound().build());
    }

}
